using Microsoft.Extensions.DependencyInjection;

namespace EtronRtuServer;
public class Program
{
    public static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        await app.RunAsync();

        var services = new ServiceCollection();

        services.AddSingleton<JsonLogger>();
        services.AddSingleton<PubService>();
        services.AddSingleton<RtuSocketServer>();

        var provider = services.BuildServiceProvider();
        
        var listener = provider.GetRequiredService<RtuSocketServer>();

        await listener.Listen();
    }
}