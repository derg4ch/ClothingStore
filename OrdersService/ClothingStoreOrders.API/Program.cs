using ClothingStoreOrders.DAL.UnitOfWork;
using ClothingStoreOrders.DAL.Repositories;
using ClothingStoreOrders.DAL.Repositories.Interfaces;
using ClothingStoreOrders.BLL.Services.Interfaces;
using ClothingStoreOrders.BLL.Services;
using ClothingStoreOrders.BLL.Automapper;
using AutoMapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using ClothingStoreOrders.DAL.db;
using Microsoft.Extensions.Logging;
using System.Threading;


namespace ClothingStoreOrders.API
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

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
                    var connectionBuilder = new SqlConnectionStringBuilder(sqlConnectionString);
                    if (string.IsNullOrEmpty(connectionBuilder.InitialCatalog))
                    {
                        connectionBuilder.InitialCatalog = "ClothingStoreOrders";
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
                throw new InvalidOperationException(
                    "SQL Server connection string is not configured. " +
                    "Please set ConnectionStrings__SqlServer environment variable or ConnectionStrings:DefaultConnection in appsettings.json. " +
                    "The application cannot start without a database connection.");
            }

            // Initialize database with retry logic
            var maxRetries = 5;
            var retryDelay = TimeSpan.FromSeconds(5);
            var success = false;
            
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var tempLogger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Program>();
                    tempLogger.LogInformation($"Database initialization attempt {attempt}/{maxRetries}...");
                    DatabaseInitializer.Initialize(sqlConnectionString);
                    success = true;
                    break;
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    var tempLogger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Program>();
                    tempLogger.LogWarning(ex, $"Database initialization attempt {attempt} failed. Retrying in {retryDelay.TotalSeconds} seconds...");
                    Thread.Sleep(retryDelay);
                }
                catch (Exception ex)
                {
                    var tempLogger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Program>();
                    tempLogger.LogError(ex, "Database initialization failed after all retry attempts.");
                }
            }
            
            if (!success)
            {
                var tempLogger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Program>();
                tempLogger.LogError("Database initialization failed after {MaxRetries} attempts. The application will continue, but database operations may fail.", maxRetries);
            }

            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(sqlConnectionString));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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


