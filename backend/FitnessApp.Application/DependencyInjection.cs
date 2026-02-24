using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Services.Admin.UserManagement.Clients;
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Services.Admin.UserManagement.Trainers;
using FitnessApp.Application.Interfaces.Authentication;
using FitnessApp.Application.Services.Authentication;
using FitnessApp.Application.Interfaces.Users;
using FitnessApp.Application.Services.Users;
using FitnessApp.Application.Factories;
using Microsoft.Extensions.DependencyInjection;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Application.Services.Subscriptions;
using FitnessApp.Application.Features.Subscriptions;
using FitnessApp.Application.Features.Statistics;
using FitnessApp.Application.Features.Workouts;

namespace FitnessApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ========== FACTORIES ==========
        services.AddScoped<ClientFactory>();
        services.AddScoped<TrainerFactory>();

        // ========== AUTHENTICATION SERVICES ==========
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // ========== USER SERVICES ==========
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IUserManagementService, UserManagementService>();

        // ========== ADMIN - CLIENT SERVICES ==========
        services.AddScoped<IClientCreationService, ClientCreationService>();
        services.AddScoped<IClientQueryService, ClientQueryService>();
        services.AddScoped<IClientUpdateService, ClientUpdateService>();
        services.AddScoped<IClientDeleteService, ClientDeleteService>();

        // ========== ADMIN - TRAINER SERVICES ==========
        services.AddScoped<ITrainerCreationService, TrainerCreationService>();
        services.AddScoped<ITrainerQueryService, TrainerQueryService>();
        services.AddScoped<ITrainerUpdateService, TrainerUpdateService>();
        services.AddScoped<ITrainerDeleteService, TrainerDeleteService>();

        // ========== SUBSCRIPTION SERVICES ==========
        services.AddScoped<IBenefitService, BenefitService>();
        services.AddScoped<IBenefitPackageService, BenefitPackageService>();
        services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IPaymentService, PaymentService>();

        // Statistics Service
        services.AddScoped<IStatisticsService, StatisticsService>();

        // ========== WORKOUT PLAN SERVICE ==========
        services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();

        return services;
    }
}