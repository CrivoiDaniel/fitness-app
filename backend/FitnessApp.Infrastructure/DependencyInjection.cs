using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Infrastructure.Repositories;
using FitnessApp.Infrastructure.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FitnessApp.Infrastructure.Data.Configurations;

namespace FitnessApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        // ========== DATABASE CONFIGURATION ==========
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(
                connectionString, 
                ServerVersion.AutoDetect(connectionString),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    );
                }
            );
            
            if (isDevelopment)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // ========== REPOSITORIES - USER MANAGEMENT ==========
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();

        // ========== REPOSITORIES - SUBSCRIPTIONS ==========
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IBenefitRepository, BenefitRepository>();
        services.AddScoped<IBenefitPackageRepository, BenefitPackageRepository>();
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
}
