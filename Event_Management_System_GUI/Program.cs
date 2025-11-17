using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Data;   // MasDbContext
using Event_Management_System_GUI.Data;   // DataSeeder erişimi için


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<MasDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MAS")));



builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MasDbContext>();
    db.Database.Migrate();

    DataSeeder.SeedIfEmptyAsync(db).GetAwaiter().GetResult();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();