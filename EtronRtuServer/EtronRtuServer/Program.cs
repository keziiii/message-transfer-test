using Microsoft.Extensions.DependencyInjection;

namespace EtronRtuServer;
public class Program
{
    public static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();

        var services = builder.Services;

        services
            .AddSingleton<JsonLogger>()
            .AddSingleton<PubService>()
            .AddSingleton<RtuSocketServer>()

            .AddControllers();

        var app = builder.Build();
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        await app.RunAsync();


        var provider = services.BuildServiceProvider();
        
        var listener = provider.GetRequiredService<RtuSocketServer>();

        await listener.Listen();
    }
}