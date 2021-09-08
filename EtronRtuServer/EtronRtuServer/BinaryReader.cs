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
