using FitnessApp.Infrastructure;
using FitnessApp.Application;

var builder = WebApplication.CreateBuilder(args);

// ========== ADD SERVICES TO THE CONTAINER ==========

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

//app.UseAuthorization();

app.MapControllers();

app.Run();