public static class Extensions
{
    public static async Task<byte[]> ReceiveAsync(this Socket socket, int length)
    {
        byte[] buffer = new byte[length];

        int rest = length;
        int receiveCount = 0;

        while (rest > 0)
        {
            var begin = socket.BeginReceive(buffer, receiveCount, rest, SocketFlags.None, null, null);
            int recv = await Task.Factory.FromAsync(begin,
                        socket.EndReceive);

            if (recv == 0) throw new Exception("recv 0");

            rest -= recv;
            receiveCount += recv;
        }
        return buffer;

    }
    public static async Task<int> SendAsync(this Socket socket, byte[] buffer)
    {
        int sendCount = await Task.Factory.FromAsync(
                   socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, socket),
                   socket.EndSend);

        return sendCount;
    }


    public static byte[] HexToBytes(string hex)
    {
        if (hex.Contains('-'))
        {
            hex = hex.Replace("-", "");
        }

        return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
    }

}

