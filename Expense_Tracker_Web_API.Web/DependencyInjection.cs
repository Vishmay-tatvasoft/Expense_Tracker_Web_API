using System.Reflection;
using System.Security.Claims;
using System.Text;
using Expense_Tracker_Web_API.Repositories.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        #region JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:EncryptKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies["ExpenseTrackerAccessToken"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Request.Headers["Authorization"] = "Bearer " + accessToken;
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    return context.Response.WriteAsync("Unauthorized");
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    return context.Response.WriteAsync("Forbidden");
                }
            };
        });
        #endregion

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
