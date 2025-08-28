using GuestBook.Models;
using GuestBook.Repositories;
using GuestBook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();