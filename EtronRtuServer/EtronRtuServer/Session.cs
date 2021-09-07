public class Session
{
    public Session(int sessionId, TcpClient tcpClient)
    {
        SessionId = sessionId;
        TcpClient = tcpClient;
    }

    public int SessionId { get; }
    public TcpClient TcpClient { get; }

    public async Task StartReceive()
    {
        var socket = this.TcpClient.Client;
        while (true)
        {
            var header = await socket.ReceiveAsync(50);

            Console.WriteLine($"header : {BitConverter.ToString(header)}");

            throw new NotImplementedException("¹Ì±¸Çö");
        }
    }
}