using Event_Management_System.Data;
using Event_Management_System.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System_GUI.Services;

public class PaymentDetailService
{
    private readonly MasDbContext _db;

    public PaymentDetailService(MasDbContext db)
    {
        _db = db;
    }

    public async Task<PaymentDetail?> GetByUserIdAsync(int userId)
    {
        return await _db.PaymentDetails.FirstOrDefaultAsync(x => x.OwnerUserId == userId);
    }

    public async Task SaveAsync(PaymentDetail detail)
    {
        _db.PaymentDetails.Update(detail);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentDetail detail)
    {
        _db.PaymentDetails.Remove(detail);
        await _db.SaveChangesAsync();
    }
}
