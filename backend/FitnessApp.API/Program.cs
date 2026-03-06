using FitnessApp.Infrastructure;
using FitnessApp.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FitnessApp.Infrastructure.Data.Seed;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ========== ADD SERVICES TO THE CONTAINER ==========

// ADD HttpContextAccessor (ÎNAINTE de Infrastructure!)
builder.Services.AddHttpContextAccessor();
// Infrastructure Layer (Database + Repositories)
builder.Services.AddInfrastructure(
    builder.Configuration,
    builder.Environment.IsDevelopment());

// Application Layer (Services + Factories)
builder.Services.AddApplication();

// Controllers
builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddOpenApi();

// ========== JWT AUTHENTICATION CONFIGURATION ==========
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

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

// AUTHENTICATION & AUTHORIZATION (ORDER MATTERS!)
app.UseAuthentication();  // ← BEFORE UseAuthorization!
app.UseAuthorization();

app.MapControllers();

// ========== SEED DATABASE (Development only) ==========
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (app.Environment.IsDevelopment())
    {
        // Apply migrations
        await context.Database.MigrateAsync();

        // Seed data
        await DatabaseSeeder.SeedAsync(context);
    }
}

app.Run();