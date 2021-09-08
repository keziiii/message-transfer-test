using Xunit;

namespace EtronRtuServer.Tests;


public class UnitTest1
{
    readonly BinaryReader binaryReader;
    public UnitTest1()
    {
        var hex = "56-76-14-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-7C-18-CD-00-0B-70-00-00-00-00-00-00-00-00-00-00-08-00-00";

        var headerBytes = Extensions.HexToBytes(hex);

        this.binaryReader = new BinaryReader(headerBytes);
    }


    [Fact]
    public void ImeiTest()
    {
        var header = binaryReader.ReadHeader();
        Assert.Equal("C18CD000B700000", header.Imei);
    }

    [Fact]
    public void HeaderCursorTest()
    {
        var header = binaryReader.ReadHeader();
        Assert.Equal(50, binaryReader.Cursor);
    }

    [Fact]
    public void Test1()
    {
        var header = binaryReader.ReadHeader();

        var ack = new RtuRequestRegistrationAck(header);

        var ackBytes = ack.ToBytes();

        Assert.Equal(61, ackBytes.Length);
    }
}