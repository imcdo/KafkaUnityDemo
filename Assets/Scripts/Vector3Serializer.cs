using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Confluent.Kafka;


public class Vector3Serializer : ISerializer<Vector3>
{
    public byte[] Serialize(Vector3 data, SerializationContext context)
    {
        using (var ms = new MemoryStream()) 
        using (var bw = new BinaryWriter(ms))
        {
            bw.Write(data.x);
            bw.Write(data.y);
            bw.Write(data.z);
            return ms.ToArray();
        }
    }
}
