namespace EtronRtuServer;
public class Program
{
    public static async Task Main()
    {
        var jsonLogger = new JsonLogger();
        var listener = new RtuSocketServer(jsonLogger);

        await listener.Listen();
    }
}