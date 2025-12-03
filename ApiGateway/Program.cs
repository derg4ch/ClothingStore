
namespace ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
            .ConfigureHttpClient((context, httpClient) =>
            {
                httpClient.ConnectTimeout = TimeSpan.FromSeconds(10);
            });

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();
        app.MapReverseProxy();

        app.Run();
    }
}
