using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : StoreContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    context.Database.Migrate();
                    seeder(context, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured during migrating the database");
                }
            }

            return host;
        }

        public static IHost MigrateIndentiyDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : AppIdentityDbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var identityContext = services.GetRequiredService<TContext>();

                try
                {
                    identityContext.Database.Migrate();
                    seeder(identityContext, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured during migrating the identity database");
                }
            }

            return host;
        }
    }
}