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

            var payload = header.BodyLength == 0 ? new byte[0] : await socket.ReceiveAsync(header.BodyLength);

            await this.OnReceive(header, payload);


        }
    }

    async Task OnReceive(RtuHeader header, byte[] payload)
    {
        Console.WriteLine($"payload : {BitConverter.ToString(payload)}");

        if(header.CommandId == 0x14)
        {
            await this.TcpClient.Client.SendAsync(new byte[] { });
        }
        else
        {
            throw new NotImplementedException("command ¹Ì±¸Çö");
        }
        
    }
}