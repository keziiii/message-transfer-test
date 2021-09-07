
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
            Imei = BitConverter.ToString(this.ReadBytes(16)[..10])[7..].Replace("-",""),
            DeviceIdLength = this.ReadByte(),
            EncryptionType = this.ReadByte(),
            StatusCode = this.ReadByte()
        };

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
    public string Imei { get; init; }
    //public byte[] DeviceId { get; init; }
    public byte DeviceIdLength { get; init; }
    public byte EncryptionType { get; init; }
    public byte StatusCode { get; init; }

}