using GuestBook.Models;
using GuestBook.Repositories;
using GuestBook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddCors();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "Session";

});

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseCors(builder => builder.WithOrigins("https://localhost:7010")
                            .AllowAnyHeader()
                            .AllowAnyMethod());
app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();