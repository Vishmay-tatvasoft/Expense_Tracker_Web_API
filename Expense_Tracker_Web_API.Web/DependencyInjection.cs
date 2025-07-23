using System.Reflection;
using Expense_Tracker_Web_API.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Web;

public class DependencyInjection
{
        public static void RegisterServices(IConfiguration configuration, IServiceCollection services, string connectionString)
    {
        services.AddControllers();
        // services.AddHttpClient();

        services.AddDbContext<ExpenseTrackerWebAPIContext>(options =>
            options.UseNpgsql(connectionString)
        );
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
                policy.WithOrigins(configuration.GetValue<string>("RequestURL:Angular")!,
                configuration.GetValue<string>("RequestURL:AngularSSR")!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });
        RegisterImplementations(services, "Expense_Tracker_Web_API.Repositories");
        RegisterImplementations(services, "Expense_Tracker_Web_API.Services");
    }

    private static void RegisterImplementations(
        IServiceCollection services,
        string assemblyName
    )
    {
        var assembly = Assembly.Load(assemblyName);
        var types = assembly.GetTypes();

        var interfaces = types.Where(t => t.IsInterface && t.Namespace is not null);
        var implementations = types.Where(t =>
            t.IsClass && !t.IsAbstract && t.Namespace is not null
        );

        foreach (var serviceInterface in interfaces)
        {
            var implementation = implementations.FirstOrDefault(implementation =>
                serviceInterface.Name[1..] == implementation.Name
            );
            if (implementation is not null)
            {
                services.AddScoped(serviceInterface, implementation);
            }
        }
    }

}
