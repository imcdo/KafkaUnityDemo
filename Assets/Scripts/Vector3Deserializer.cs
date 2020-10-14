using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Confluent.Kafka;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Vector3Deserializer : IDeserializer<Vector3>
{
    public Vector3 Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        float x, y, z;
        
        using (var ms = new MemoryStream(data.ToArray()))
        using (var br = new BinaryReader(ms))
        {
            x = br.ReadSingle();
            y = br.ReadSingle();
            z = br.ReadSingle();
            
        }
        return new Vector3(x, y, z);
    }
}
