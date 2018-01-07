using System.IO;
using Avro.Generic;
using Avro.IO;

namespace KafkaAvroNet.Avro
{
    public static class Helper
    {
        
        public static uint SwapEndianness(uint x)
        {
            return ((x & 0x000000ff) << 24) + // First byte
                   ((x & 0x0000ff00) << 8) + // Second byte
                   ((x & 0x00ff0000) >> 8) + // Third byte
                   ((x & 0xff000000) >> 24); // Fourth byte
        }

        public static void AvroEncodeInt(BinaryWriter stream, int x)
        {
            BinaryEncoder _e = new BinaryEncoder(stream.BaseStream);
            _e.WriteInt(x);
        }

        public static int AvroDecodeInt(BinaryReader stream)
        {
            BinaryDecoder _e = new BinaryDecoder(stream.BaseStream);
            return _e.ReadInt();
        }
    }
}
