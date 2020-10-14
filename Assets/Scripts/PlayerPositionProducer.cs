using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Confluent.Kafka;
using System.Net;


public class PlayerPositionProducer : MonoBehaviour
{
    private IProducer<int, Vector3> _producer;
    public int PlayerNumber; 
    void Start()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = Dns.GetHostName(),
            Acks=0
            
        };

        var pb = new ProducerBuilder<int, Vector3>(config);
        pb.SetValueSerializer(new Vector3Serializer());
        _producer =pb.Build();
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void LateUpdate()
    {
        _producer.Produce("player-pos", new Message<int, Vector3>{Key = PlayerNumber, Value = transform.position});
    }

    private void OnDestroy()
    {
        _producer.Dispose();
    }
}
