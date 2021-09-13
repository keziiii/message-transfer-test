using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EtronRtuServer.Tests;

public class ServiceTests
{
    IServiceProvider provider;
    public ServiceTests()
    {
        var services = new ServiceCollection();

        services.AddSingleton<JsonLogger, MockJsonLogger>();
        services.AddSingleton<PubService>();
        services.AddSingleton<RtuSocketServer>();

        this.provider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task ListenLogTest()
    {
        var server = provider.GetRequiredService<RtuSocketServer>();
        _=server.Listen();

        await Task.Delay(500);

        var jsonLogger = (MockJsonLogger)provider.GetRequiredService<JsonLogger>();

        Assert.Single(jsonLogger.Logs);

        var log = jsonLogger.Logs.First();
    }


    [Fact]
    public void JsonLoggerTest()
    {
        var jsonLogger = (MockJsonLogger)provider.GetRequiredService<JsonLogger>();

        jsonLogger.Write("test", new { a = 1, b = 2 });

        Assert.Single(jsonLogger.Logs);

        var log = jsonLogger.Logs.First();

        Assert.Equal("test", (string)log["subject"]);
        Assert.Equal(1, (int)log["a"]);
        Assert.Equal(2, (int)log["b"]);
        Assert.NotNull(log["time"]);
    }
}

