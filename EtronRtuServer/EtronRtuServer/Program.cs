Console.WriteLine("Hello, World!");

int port = 7000;
var listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
listener.Start();

int sessionId = 0;
while (true)
{
    var tcpClient = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

    sessionId++;

    Console.WriteLine($"connected : {sessionId}"); //접속됨.
    var session = new Session(sessionId, tcpClient);
}

