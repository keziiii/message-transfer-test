namespace EtronRtuServer;

public class RtuSocketServer
{

    public Dictionary<int, Session> Sessions = new ();

    public async Task Listen()
    {
        int port = 7000;
        var listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
        listener.Start();

        int sessionId = 0;
        while (true)
        {
            var tcpClient = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

            _ = OnAccepted(sessionId++, tcpClient);
        }
    }

    public async Task OnAccepted(int sessionId, TcpClient tcpClient)
    {
        Console.WriteLine($"connected : {sessionId}"); //접속됨.

        var session = new Session(sessionId, tcpClient);

        Sessions.Add(sessionId, session);

        await session.StartReceive();

        Console.WriteLine($"finished : {session.SessionId}, removed : {Sessions.Remove(session.SessionId)}");
    }
}