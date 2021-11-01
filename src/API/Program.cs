using API.Extensions;
using Core.Entities.Identitiy;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<StoreContext>((context, services) => 
                {
                    var logger = services.GetService<ILogger<StoreContextSeed>>();
                    StoreContextSeed
                        .SeedAsync(context, logger)
                        .Wait();
                })
                .MigrateIndentiyDatabase<AppIdentityDbContext>((identityContext, services) => 
                {
                    var logger = services.GetService<ILogger<AppIdentityDbContextSeed>>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    AppIdentityDbContextSeed
                        .SeedUserAsync(userManager)
                        .Wait();
                }) 
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}