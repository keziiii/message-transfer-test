
using static BinaryEncoding.Binary;

public class BinaryReader
{
    private readonly byte[] data;
    int cursor;
    public BinaryReader(byte[] data)
    {
        this.data = data;
    }

    public byte ReadByte() => data[cursor++];
    public byte[] ReadBytes(int length)=> data[cursor.. (cursor+=length)];

    public RtuHeader ReadHeader()
    {
        var result = new RtuHeader()
        {
            Version = this.ReadBytes(2),
            CommandId = this.ReadByte(),
            Token = this.ReadBytes(20),
            BodyLength = Binary.LittleEndian.GetUInt16(this.ReadBytes(2)),
            TransactionId = this.ReadBytes(2),
            ModelCode = this.ReadBytes(2),
            DeviceId = this.ReadBytes(16),
            DeviceIdLength = this.ReadByte(),
            EncryptionType = this.ReadByte(),
            StatusCode = this.ReadByte()
        };

        result.Imei = BitConverter.ToString(result.DeviceId[..10])[7..].Replace("-", "");
        
        return result;
    }
}

public struct RtuHeader
{
    public byte[] Version { get; init; }
    public byte CommandId { get; init; }
    public byte[] Token { get; init; }
    public UInt16 BodyLength { get; init; }
    public byte[] TransactionId { get; init; }
    public byte[] ModelCode { get; init; }
    
    public byte[] DeviceId { get; init; }
    public string Imei { get; set; }
    public byte DeviceIdLength { get; init; }
    public byte EncryptionType { get; init; }
    public byte StatusCode { get; init; }

}

public struct RtuRequestRegistrationAck
{
    public RtuRequestRegistrationAck(RtuHeader header)
    {

    }

    public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream();

        memoryStream.WriteByte(1);
        memoryStream.Write(new byte[] { 1, 2 });
        throw new NotImplementedException();

        return memoryStream.ToArray();
    }
}