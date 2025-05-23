using Exercices.Exo01.Data;
using Exercices.Exo01.Entities;
using Exercices.Exo01.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exercices.Exo01.Extensions
{
    public static class DataExtension
    {
        public static void AddDataDependencies(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            services.AddScoped<IRepository<LoginAttempt, long>, LoginAttemptRepository>();
            services.AddScoped<IRepository<AppUser, long>, AppUserRepository>();
        }
    }
}
