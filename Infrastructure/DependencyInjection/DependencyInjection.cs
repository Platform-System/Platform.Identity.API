using Microsoft.EntityFrameworkCore;
using Platform.Application.DependencyInjection;
using Platform.Identity.API.Infrastructure.Data;
using Platform.Infrastructure.DependencyInjection;
using Platform.SystemContext.DependencyInjection;
using System.Reflection;
using Platform.BuildingBlocks.DependencyInjection;
using Platform.Infrastructure.Data;
using Platform.Identity.API.Application.Abstractions;
using Platform.Identity.API.Infrastructure.Keycloak;
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
            services.Configure<KeycloakAdminOptions>(configuration.GetSection(KeycloakAdminOptions.SectionName));
            services.AddHttpClient<IKeycloakAdminClient, KeycloakAdminClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<KeycloakAdminOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/'));
            });
            
            services.AddSingleton<IConnectionMultiplexer>(sp => 
                ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));

            services.AddInfrastructure(configuration);
            return services;
        }
    }
}
