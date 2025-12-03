
using ClothingStore.DAL.Db;
using ClothingStore.DAL.Repositories.Intarfaces;
using ClothingStore.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using ClothingStore.BLL.Automapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ClothingStore.BLL.Services.Interfaces;
using ClothingStore.BLL.Services;
using ClothingStore.DAL.UOW;
using FluentValidation.AspNetCore;
using FluentValidation;
using ClothingStore.BLL.DTO;
using ClothingStore.BLL.Validation;
using ClothingStore.API.Middleware;
using Microsoft.Data.SqlClient;
namespace ClothingStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.AddServiceDefaults();


            // Add services to the container.
            var sqlConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__SqlServer");
            
            // Fallback to appsettings.json if environment variable is not set
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            }

            // Ensure database name is set in connection string (for Aspire)
            if (!string.IsNullOrEmpty(sqlConnectionString))
            {
                try
                {
                    var connectionBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(sqlConnectionString);
                    if (string.IsNullOrEmpty(connectionBuilder.InitialCatalog))
                    {
                        connectionBuilder.InitialCatalog = "ClothingStore";
                        sqlConnectionString = connectionBuilder.ConnectionString;
                    }
                }
                catch
                {
                    // If connection string parsing fails, continue with original string
                }
            }

            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                // If connection string is not configured, throw exception with clear message
                throw new InvalidOperationException(
                    "SQL Server connection string is not configured. " +
                    "Please set ConnectionStrings__SqlServer environment variable or ConnectionStrings:DefaultConnection in appsettings.json. " +
                    "The application cannot start without a database connection.");
            }

            builder.Services.AddDbContext<ClothingStoreContext>(options =>
                options.UseSqlServer(sqlConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            builder.Services.AddScoped<IProductSupplierRepository, ProductSupplierRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
            builder.Services.AddScoped<IProductSupplierService, ProductSupplierService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<CategoryDto>, CategoryCreateDtoValidator>();
            builder.Services.AddScoped<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            builder.Services.AddScoped<IValidator<ProductDetailCreateDto>, ProductDetailCreateDtoValidator>();
            builder.Services.AddScoped<IValidator<ProductSupplierDto>, ProductSupplierCreateValidator>();
            builder.Services.AddScoped<IValidator<SupplierCreateDto>, SupplierCreateDtoValidator>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Initialize database with error handling
            if (!string.IsNullOrEmpty(sqlConnectionString))
            {
                try
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ClothingStoreContext>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        
                        logger.LogInformation("Starting database initialization...");
                        logger.LogInformation($"Connection string configured: Server={(sqlConnectionString.Contains("Server=") ? sqlConnectionString.Split(';').FirstOrDefault(s => s.StartsWith("Server=")) : "Not found")}");
                        
                        // Retry logic for database connection
                        var maxRetries = 5;
                        var retryDelay = TimeSpan.FromSeconds(5);
                        var success = false;
                        
                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                logger.LogInformation($"Database connection attempt {attempt}/{maxRetries}...");
                                
                                // Try to ensure database exists first (if connection string allows)
                                try
                                {
                                    // EnsureCreated will create the database if it doesn't exist
                                    logger.LogInformation("Ensuring database exists...");
                                    var created = context.Database.EnsureCreated();
                                    if (created)
                                    {
                                        logger.LogInformation("Database was created successfully.");
                                    }
                                    else
                                    {
                                        logger.LogInformation("Database already exists.");
                                    }
                                }
                                catch (Exception ensureEx)
                                {
                                    logger.LogWarning(ensureEx, $"EnsureCreated failed (may need to create database manually): {ensureEx.Message}");
                                    // Try to continue with migrations
                                }
                                
                                // Check if we can connect
                                try
                                {
                                    logger.LogInformation("Checking database connection...");
                                    var canConnect = context.Database.CanConnect();
                                    if (!canConnect)
                                    {
                                        throw new Exception("Cannot connect to database - CanConnect() returned false");
                                    }
                                    logger.LogInformation("Database connection successful.");
                                }
                                catch (Exception connectEx)
                                {
                                    logger.LogError(connectEx, $"Database connection check failed: {connectEx.Message}");
                                    throw;
                                }
                                
                                // Apply migrations if they exist
                                try
                                {
                                    logger.LogInformation("Checking for pending migrations...");
                                    var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                                    if (pendingMigrations.Any())
                                    {
                                        logger.LogInformation($"Found {pendingMigrations.Count} pending migrations. Applying...");
                                        context.Database.Migrate();
                                        logger.LogInformation("Migrations applied successfully.");
                                    }
                                    else
                                    {
                                        logger.LogInformation("No pending migrations found.");
                                    }
                                }
                                catch (Exception migrationEx)
                                {
                                    logger.LogError(migrationEx, $"Migration failed: {migrationEx.Message}");
                                    logger.LogError(migrationEx, $"Exception type: {migrationEx.GetType().Name}");
                                    if (migrationEx.InnerException != null)
                                    {
                                        logger.LogError(migrationEx, $"Inner exception: {migrationEx.InnerException.Message}");
                                    }
                                    // Don't throw - migrations might not be needed if EnsureCreated worked
                                }
                                
                                // Seed database
                                try
                                {
                                    logger.LogInformation("Starting database seeding...");
                                    Seeding.SeedAsync(context).GetAwaiter().GetResult();
                                    logger.LogInformation("Database seeding completed successfully.");
                                }
                                catch (Exception seedEx)
                                {
                                    logger.LogError(seedEx, $"Database seeding failed: {seedEx.Message}");
                                    // Don't throw - seeding failure is not critical
                                }
                                
                                logger.LogInformation("Database initialization completed successfully.");
                                success = true;
                                break;
                            }
                            catch (Exception ex) when (attempt < maxRetries)
                            {
                                logger.LogWarning(ex, $"Database initialization attempt {attempt} failed: {ex.Message}. Retrying in {retryDelay.TotalSeconds} seconds...");
                                logger.LogWarning(ex, $"Exception type: {ex.GetType().Name}");
                                if (ex.InnerException != null)
                                {
                                    logger.LogWarning(ex, $"Inner exception: {ex.InnerException.Message}");
                                }
                                Thread.Sleep(retryDelay);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, $"Database initialization attempt {attempt} failed with non-retryable error: {ex.Message}");
                                logger.LogError(ex, $"Exception type: {ex.GetType().Name}");
                                logger.LogError(ex, $"Full exception stack trace: {ex.StackTrace}");
                                throw; // Re-throw to be caught by outer catch
                            }
                        }
                        
                        if (!success)
                        {
                            logger.LogError("Failed to initialize database after {MaxRetries} attempts.", maxRetries);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = app.Services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database. The application will continue to start.");
                    logger.LogError(ex, $"Exception type: {ex.GetType().Name}");
                    logger.LogError(ex, $"Exception message: {ex.Message}");
                    logger.LogError(ex, $"Inner exception: {ex.InnerException?.Message ?? "None"}");
                    logger.LogError(ex, $"Stack trace: {ex.StackTrace}");
                    // Don't throw - let the app start and show the error in logs
                }
            }
            else
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Skipping database initialization - connection string is not configured.");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
