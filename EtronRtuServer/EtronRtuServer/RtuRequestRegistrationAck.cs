﻿
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
    private byte[] Version;
    private byte CommandId;
    private byte[] Token;
    private UInt16 BodyLength;
    private byte[] TransactionId;
    private byte[] ModelCode;
    private byte[] DeviceId;
    private byte DeviceIdLength;
    private byte EncryptionType;
    private byte StatusCode;
    private byte AlwaysOn;
    private byte[] StartTime;
    private byte[] EndTime;
    private UInt16 HeartBeatInterval;
    private byte[] CurrentTime;

    public RtuRequestRegistrationAck(RtuHeader header)
    {
        this.Version = header.Version; // 고정값
        this.CommandId = 0x15; // 고정값
        this.Token = new byte[20];        
        this.BodyLength = 11;
        this.TransactionId = new byte[2]; // 사용x
        this.ModelCode = new byte[2]; // ??
        this.DeviceId = header.DeviceId;
        this.DeviceIdLength = header.DeviceIdLength;
        this.EncryptionType = 0;
        this.StatusCode = 0;
        this.AlwaysOn = 0; // 사용x
        this.StartTime = new byte[2]; // 사용x
        this.EndTime = new byte[2]; // 사용x
        this.HeartBeatInterval = (ushort)3;
        this.CurrentTime = new byte[4]; // 사용x

        this.GenerateToken();
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
        memoryStream.Write(BitConverter.GetBytes(this.HeartBeatInterval));
        memoryStream.Write(this.CurrentTime);
        
        var temp = memoryStream.ToArray();
        Console.WriteLine("---------");
        foreach (var VARIABLE in temp)
        {
            Console.Write("{0:x}",VARIABLE);
        }
        Console.WriteLine();
        Console.WriteLine("---------");
        
        return temp;
    }

    public void GenerateToken()
    {
        Random rnd = new Random();
        rnd.NextBytes(this.Token);
    }
}