using Microsoft.EntityFrameworkCore;
using Platform.Application.DependencyInjection;
using Platform.Identity.API.Infrastructure.Data;
using Platform.Infrastructure.DependencyInjection;
using Platform.SystemContext.DependencyInjection;
using System.Reflection;
using Platform.BuildingBlocks.DependencyInjection;
using Platform.Infrastructure.Data;
using StackExchange.Redis;

namespace Platform.Identity.API.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<BaseDbContext>(sp => sp.GetRequiredService<IdentityDbContext>());

            services.AddBuildingBlocks();
            services.AddApplication(assembly);
            services.AddSystemContext();
            
            services.AddSingleton<IConnectionMultiplexer>(sp => 
                ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));

            services.AddInfrastructure(configuration);
            return services;
        }
    }
}
