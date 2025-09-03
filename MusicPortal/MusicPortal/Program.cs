using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.Extensions;
using MusicPortal.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddMusicPortalServices(connection);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Songs}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();