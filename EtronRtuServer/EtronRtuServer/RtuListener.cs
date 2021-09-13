namespace EtronRtuServer;

public class RtuListener
{
    private readonly JsonLogger jsonLogger;

    public RtuListener(JsonLogger jsonLogger)
    {
        this.jsonLogger = jsonLogger;
    }

    public async Task Listen()
    {
        int port = 7000;
        var listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
        listener.Start();

        int sessionId = 0;
        while (true)
        {
            var tcpClient = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

            sessionId++;

            this.jsonLogger.Write("connected", new
            {
                sessionId
            }); //접속됨.

            var session = new Session(sessionId, tcpClient);

            _ = session.StartReceive();
        }
    }
}