namespace EtronRtuServer;
/*
Command ID
(0x02) Response Get Device Status 
(0x03) Response Ack 
(0x14) Request Registration 
(0x15) Response Request Registration 
(0x1B) Control Order

sample header:
56-76-14-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-7C-18-CD-00-0B-70-00-00-00-00-00-00-00-00-00-00-08-00-00
*/
public struct RtuRequestRegistrationAck
{
    private readonly byte[] Version;
    private readonly byte CommandId;
    private readonly byte[] Token;
    private readonly UInt16 BodyLength;
    private readonly byte[] TransactionId;
    private readonly byte[] ModelCode;
    private readonly byte[] DeviceId;
    private readonly byte DeviceIdLength;
    private readonly byte EncryptionType;
    private readonly byte StatusCode;
    private readonly byte AlwaysOn;
    private readonly byte[] StartTime;
    private readonly byte[] EndTime;
    private readonly UInt16 HeartBeatInterval;
    private readonly byte[] CurrentTime;

    public RtuRequestRegistrationAck(RtuHeader header)
    {
        this.Version = header.Version; // 고정값
        this.CommandId = 0x15; // 고정값

        this.Token = new byte[20];
        var rnd = new Random();
        rnd.NextBytes(this.Token);

        this.BodyLength = 11;
        this.TransactionId = new byte[2]; // 사용x
        this.ModelCode = header.ModelCode;
        this.DeviceId = header.DeviceId;
        this.DeviceIdLength = header.DeviceIdLength;
        this.EncryptionType = 0;
        this.StatusCode = 0;
        this.AlwaysOn = 0; // 사용x
        this.StartTime = new byte[2]; // 사용x
        this.EndTime = new byte[] { 23, 59 };
        this.HeartBeatInterval = (ushort)3;
        this.CurrentTime = new byte[4]; // 사용x
    }

    public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream();

        memoryStream.Write(this.Version);
        memoryStream.WriteByte(this.CommandId);
        memoryStream.Write(this.Token);
        memoryStream.Write(Binary.LittleEndian.GetBytes(this.BodyLength));
        memoryStream.Write(this.TransactionId);
        memoryStream.Write(this.ModelCode);
        memoryStream.Write(this.DeviceId);
        memoryStream.WriteByte(this.DeviceIdLength);
        memoryStream.WriteByte(this.EncryptionType);
        memoryStream.WriteByte(this.StatusCode);
        memoryStream.WriteByte(this.AlwaysOn);
        memoryStream.Write(this.StartTime);
        memoryStream.Write(this.EndTime);

        memoryStream.Write(Binary.LittleEndian.GetBytes(this.HeartBeatInterval));
        memoryStream.Write(this.CurrentTime);
        
        var result = memoryStream.ToArray();
        
        return result;
    }

}