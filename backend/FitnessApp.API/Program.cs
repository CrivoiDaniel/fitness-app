using Microsoft.EntityFrameworkCore;
using FitnessApp.Infrastructure.Data;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Infrastructure.Repositories;
using FitnessApp.Application.Interfaces.Authentication;
using FitnessApp.Application.Services.Authentication;
using FitnessApp.Application.Interfaces.Users;
using FitnessApp.Application.Services.Users;
using FitnessApp.Application.Interfaces.Admin;
using FitnessApp.Application.Services.Admin;
using FitnessApp.Application.Factories;

var builder = WebApplication.CreateBuilder(args);

// ========== ADD SERVICES TO THE CONTAINER ==========

builder.Services.AddControllers();

// OpenAPI/Swagger for .NET 10
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

// Admin services
builder.Services.AddScoped<IClientCreationService, ClientCreationService>();
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

// Swagger for .NET 10
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();