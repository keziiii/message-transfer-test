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
           
            var headerBytes = await socket.ReceiveAsync(50);
            Console.WriteLine($"header : {BitConverter.ToString(headerBytes)}");

            var headerReader = new BinaryReader(headerBytes);
            var header = headerReader.ReadHeader();

            var payloadBytes = header.BodyLength == 0 ? new byte[0] : await socket.ReceiveAsync(header.BodyLength);

            Console.WriteLine($"payload : {BitConverter.ToString(payloadBytes)}");

            throw new NotImplementedException("¹Ì±¸Çö");
        }
    }
}