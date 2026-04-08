using FitnessApp.Infrastructure;
using FitnessApp.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FitnessApp.Infrastructure.Data.Seed;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
builder.Services.AddControllers().AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;

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
        try 
        {
            // 1. Sync migration history if it's broken
            await context.Database.ExecuteSqlRawAsync("INSERT IGNORE INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260329140449_AddPaymentGatewayLogs', '9.0.0');");
            
            // 2. Safely add Google columns to users table (MySQL syntax)
            await context.Database.ExecuteSqlRawAsync("SET @col_exists = (SELECT COUNT(*) FROM information_schema.columns WHERE table_name = 'users' AND table_schema = DATABASE() AND column_name = 'GoogleEmail'); SET @sql = IF(@col_exists = 0, 'ALTER TABLE users ADD COLUMN GoogleEmail LONGTEXT NULL', 'SELECT 1'); PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;");
            await context.Database.ExecuteSqlRawAsync("SET @col_exists = (SELECT COUNT(*) FROM information_schema.columns WHERE table_name = 'users' AND table_schema = DATABASE() AND column_name = 'GoogleRefreshToken'); SET @sql = IF(@col_exists = 0, 'ALTER TABLE users ADD COLUMN GoogleRefreshToken LONGTEXT NULL', 'SELECT 1'); PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;");

            // 3. Apply all pending migrations (including the new Appointments table)
            await context.Database.MigrateAsync();
            
            // 4. Seed data (including Ion trainer)
            await DatabaseSeeder.SeedAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DB SYNC ERROR] {ex.Message}");
        }
    }
}

app.Run();