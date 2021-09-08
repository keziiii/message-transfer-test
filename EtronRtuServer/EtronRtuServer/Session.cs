namespace EtronRtuServer;
public class Session
{
    public Session(int sessionId, TcpClient tcpClient)
    {
        SessionId = sessionId;
        TcpClient = tcpClient;
    }

    public int SessionId { get; }
    public TcpClient TcpClient { get; }

    public string Imei { get; private set; }

    public async Task StartReceive()
    {
        var socket = this.TcpClient.Client;
        while (true)
        {
            try
            {
                var headerBytes = await socket.ReceiveAsync(50);
                Console.WriteLine($"header : {BitConverter.ToString(headerBytes)}");

                var headerReader = new BinaryReader(headerBytes);
                var header = headerReader.ReadHeader();

                var payload = header.BodyLength == 0 ? Array.Empty<byte>() : await socket.ReceiveAsync(header.BodyLength);

                await this.OnReceive(header, payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                break;
            }
        }
    }


    static readonly byte[] statusAckBytes = new byte[] { 0x56, 0x76, 0x03 };

    async Task OnReceive(RtuHeader header, byte[] payload)
    {
        Console.WriteLine($"payload : {BitConverter.ToString(payload)}");

        if(header.CommandId == 0x14)
        {
            this.Imei = header.Imei;

            //db 확인
            //첫접속에 대한 확인+처리

            var ack = new RtuRequestRegistrationAck(header);
            await this.TcpClient.Client.SendAsync(ack.ToBytes());
        }
        else if(header.CommandId == 0x02)
        {
            Console.WriteLine($"{payload.Length} >> {BitConverter.ToString(payload)}");
            await this.TcpClient.Client.SendAsync(statusAckBytes);
        }
        else
        {
            throw new NotImplementedException("command 미구현");
        }
        
    }
}