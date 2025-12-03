using AggregatorService.Services;
using Microsoft.Extensions.Hosting;

namespace AggregatorService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        // Add services to the container.
        // Use service discovery through Aspire
        builder.Services.AddHttpClient("orders", (sp, client) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var ordersUrl = config["Services:orders-service"] ?? "https://localhost:5003";
            client.BaseAddress = new Uri(ordersUrl);
        });

        builder.Services.AddHttpClient("reviews", (sp, client) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var reviewsUrl = config["Services:reviews-service"] ?? "https://localhost:5002";
            client.BaseAddress = new Uri(reviewsUrl);
        });

        builder.Services.AddHttpClient("catalog", (sp, client) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var catalogUrl = config["Services:catalog-service"] ?? "https://localhost:5001";
            client.BaseAddress = new Uri(catalogUrl);
        });

        builder.Services.AddScoped<IAggregationService, AggregationService>();

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
