using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger intentionally not registered to avoid OS/application control policy blocks for Swashbuckle.
// Enable OpenAPI by adding the Swashbuckle.AspNetCore package back to the project and ensuring
// the runtime is permitted to load its assemblies. For now, we avoid referencing it so the
// application can run even when AppLocker/WDAC blocks NuGet assemblies.
var enableOpenApi = builder.Configuration.GetValue<bool>("EnableOpenApi", false);
if (enableOpenApi)
{
    Console.WriteLine("EnableOpenApi was requested, but Swashbuckle package is not referenced. Add package and ensure assemblies are allowed by OS policies.");
}

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HRSystembackend.Data.HRDbContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Note: Swagger middleware is not registered because Swashbuckle package was removed to avoid
// OS/application control policy issues. Re-add Swashbuckle and update configuration to enable it.

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();