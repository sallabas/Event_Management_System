using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

public class PromotionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private static readonly Queue<int> _pendingRequests = new();

    public PromotionBackgroundService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static Task AddPendingRequestAsync(int requestId)
    {
        _pendingRequests.Enqueue(requestId);
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MasDbContext>();

                // pending to expired
                while (_pendingRequests.Count > 0)
                {
                    var requestId = _pendingRequests.Dequeue();

                    var req = await db.PromotedRequests
                        .FirstOrDefaultAsync(r => r.PromotedRequestId == requestId, stoppingToken);

                    if (req != null && req.Status == PromotionStatus.Pending)
                    {
                        await Task.Delay(4000, stoppingToken); // simulate payment confirmation delay
                        req.UpdateStatus(PromotionStatus.Confirmed);
                    }
                }

                // confirmed to expired 
                var expiredRequests = await db.PromotedRequests
                    .Where(r =>
                        r.Status == PromotionStatus.Confirmed &&
                        r.PromotionEndDate <= DateTime.Now)
                    .ToListAsync(stoppingToken);

                foreach (var req in expiredRequests)
                {
                    req.UpdateStatus(PromotionStatus.Expired);
                }

                await db.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}