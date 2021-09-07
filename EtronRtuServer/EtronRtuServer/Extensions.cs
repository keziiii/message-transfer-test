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
}

