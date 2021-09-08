namespace EtronRtuServer;
public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        var listener = new RtuSocketServer();

        await listener.Listen();
    }
}