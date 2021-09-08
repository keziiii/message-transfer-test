using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using BinaryEncoding;
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
        memoryStream.Write(BitConverter.GetBytes(this.BodyLength));
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