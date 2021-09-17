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
        
        var listener = app.Services.GetRequiredService<RtuSocketServer>();

        await Task.WhenAll(app.RunAsync(), listener.Listen());
    }
}