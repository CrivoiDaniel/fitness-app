using Microsoft.EntityFrameworkCore;
using FitnessApp.Infrastructure.Data;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Infrastructure.Repositories;

// ========== USING STATEMENTS - ADMIN CLIENTS ==========
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Services.Admin.UserManagement.Clients;

// ========== USING STATEMENTS - ADMIN TRAINERS ==========
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Services.Admin.UserManagement.Trainers;

// ========== USING STATEMENTS - AUTHENTICATION ==========
using FitnessApp.Application.Interfaces.Authentication;

// ========== USING STATEMENTS - USERS ==========
using FitnessApp.Application.Interfaces.Users;
using FitnessApp.Application.Services.Users;

// ========== USING STATEMENTS - FACTORIES ==========
using FitnessApp.Application.Factories;
using FitnessApp.Application.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// ========== ADD SERVICES TO THE CONTAINER ==========

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ========== DATABASE CONFIGURATION ==========

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
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
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// ========== DEPENDENCY INJECTION - REPOSITORIES ==========

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

// ========== DEPENDENCY INJECTION - FACTORIES ==========

builder.Services.AddScoped<ClientFactory>();
builder.Services.AddScoped<TrainerFactory>();

// ========== DEPENDENCY INJECTION - SERVICES ==========

// Authentication services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// User services
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

// ✅ Admin - Client services (ADD THESE!)
builder.Services.AddScoped<IClientCreationService, ClientCreationService>();
builder.Services.AddScoped<IClientQueryService, ClientQueryService>();
builder.Services.AddScoped<IClientUpdateService, ClientUpdateService>();   
builder.Services.AddScoped<IClientDeleteService, ClientDeleteService>();  

// Admin - Trainer services
builder.Services.AddScoped<ITrainerCreationService, TrainerCreationService>();

// ========== CORS CONFIGURATION ==========

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ========== CONFIGURE THE HTTP REQUEST PIPELINE ==========

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Fitness App API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();