using Event_Management_System_GUI.Services;
using Event_Management_System.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var basePath = AppContext.BaseDirectory;  // bin/Debug/net8.0/
var solutionRoot = Path.GetFullPath(Path.Combine(basePath, @"../../../"));
var dbPath = Path.Combine(solutionRoot, "Event_Management_System", "mas.db");

Console.WriteLine("DB PATH (GUI) => " + dbPath);

builder.Services.AddDbContext<MasDbContext>();

/*
builder.Services.AddDbContext<MasDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
    */

// API bağlantısı
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:5001") 
});

// Services
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<PromotionService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();