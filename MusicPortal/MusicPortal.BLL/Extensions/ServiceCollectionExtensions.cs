using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicPortal.DAL.EF;        
using MusicPortal.DAL.Interfaces;        
using MusicPortal.DAL.Repositories;    
using MusicPortal.BLL.Services; 
using MusicPortal.BLL.Mappings;
using MusicPortal.DAL.Entities;
using MusicPortal.BLL.Interfaces;

namespace MusicPortal.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMusicPortalServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MusicPortalContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
    }
}