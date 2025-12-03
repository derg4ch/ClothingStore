using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Використовуємо SQL Server в Docker контейнері
// Aspire автоматично налаштовує необхідні параметри
var sql = builder.AddSqlServer("sqlserver")
    .WithDataVolume();

// Використовуємо MongoDB в Docker контейнері
var mongo = builder.AddMongoDB("mongo")
    .WithImage("mongo:7")
    .WithDataVolume("mongo");




var catalogService = builder.AddProject<Projects.ClothingStore_API>("catalog-service")
    .WithReference(sql)
    .WaitFor(sql);

var ordersService = builder.AddProject<Projects.ClothingStoreOrders_API>("orders-service")
    .WithReference(sql)
    .WaitFor(sql);

var reviewsService = builder.AddProject<Projects.WebApi>("reviews-service")
    .WithReference(mongo)
    .WaitFor(mongo);

var apiGateway = builder.AddProject<Projects.ApiGateway>("gateway")
    .WithReference(catalogService)
    .WithReference(ordersService)
    .WithReference(reviewsService)
    .WithExternalHttpEndpoints();

var aggregationApi = builder.AddProject<Projects.AggregatorService>("aggregation-service")
    .WithReference(ordersService)
    .WithReference(reviewsService)
    .WaitFor(sql)
    .WaitFor(mongo);




builder.Build().Run();