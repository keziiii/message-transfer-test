namespace EtronRtuServer;

public class RtuSocketServer
{

    public RtuSocketServer(JsonLogger jsonLogger)
    {
        this.jsonLogger = jsonLogger;
    }

    public Dictionary<int, Session> Sessions = new ();
    private readonly JsonLogger jsonLogger;

    public async Task Listen()
    {
        int port = 7000;
        var listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));

        listener.Start();

        this.jsonLogger.Write("listener started", new
        {
            port
        }); //접속됨.

        int sessionId = 0;
        while (true)
        {
            var tcpClient = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

            _ = OnAccepted(sessionId++, tcpClient);
        }
    }

    public async Task OnAccepted(int sessionId, TcpClient tcpClient)
    {
        this.jsonLogger.Write("connected", new
        {
            sessionId
        }); //접속됨.
        var session = new Session(sessionId, tcpClient);

        Sessions.Add(sessionId, session);

        await session.StartReceive();

        this.jsonLogger.Write("disconnected", new
        {
            session.SessionId,
            removed = Sessions.Remove(session.SessionId)
        }); //접속종료.
    }
}