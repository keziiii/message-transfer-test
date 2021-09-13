namespace EtronRtuServer;
public class Session
{
    public Session(int sessionId, TcpClient tcpClient)
    {
        SessionId = sessionId;
        TcpClient = tcpClient;
        this.jsonSessionLogger = new JsonSessionLogger(this);
        this.ConnectedTime = DateTime.Now;
    }

    public int SessionId { get; }
    public DateTime ConnectedTime { get; set; }
    public TcpClient TcpClient { get; }

    public string? Imei { get; private set; }

    public async Task StartReceive()
    {
        var socket = this.TcpClient.Client;
        while (true)
        {
            try
            {
                var headerBytes = await socket.ReceiveAsync(50);

                var headerReader = new BinaryReader(headerBytes);
                var header = headerReader.ReadHeader();

                this.jsonSessionLogger.Write("receive header", new
                {
                    raw = BitConverter.ToString(headerBytes),
                    header
                });

                var payload = header.BodyLength == 0 ? Array.Empty<byte>() : await socket.ReceiveAsync(header.BodyLength);

                if(payload.Length > 0)
                {
                    this.jsonSessionLogger.Write("receive payload", new
                    {
                        header,
                        payload
                    });
                }


                await this.OnReceive(header, payload);
            }
            catch (Exception ex)
            {
                this.jsonSessionLogger.Write("session exception", new
                {
                    error = ex.Message,
                    stacktrace = ex.StackTrace
                });

                break;
            }
        }
    }


    static readonly byte[] statusAckBytes = new byte[] { 0x56, 0x76, 0x03 };
    private readonly JsonSessionLogger jsonSessionLogger;

    async Task OnReceive(RtuHeader header, byte[] payload)
    {
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
            this.jsonSessionLogger.Write("receive request registration", new
            {
                
            });
            await this.TcpClient.Client.SendAsync(statusAckBytes);
        }
        else
        {
            throw new NotImplementedException("command 미구현");
        }
        
    }
}