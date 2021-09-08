namespace EtronRtuServer;
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
