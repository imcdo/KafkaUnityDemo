using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Confluent.Kafka;

public class PlayerPositionConsumer : MonoBehaviour
{
    private struct PlayerPositionSubscriberData
    {
        public PlayerPositionSubscriber Player;
        public float time;
    }
    
    private IConsumer<int, Vector3> _consumer;
    [SerializeField] private PlayerPositionSubscriber _subscriberPrefab;

    private Vector3 _pos;
    private Thread _poller;
    private bool _pollerRunning = true;

    private Dictionary<int, PlayerPositionSubscriber> _playerPositionSubscribers = new Dictionary<int, PlayerPositionSubscriber>();
    private ConcurrentQueue<Message<int, Vector3>> _toProcess = new ConcurrentQueue<Message<int, Vector3>>();


    void PollerLoop()
    {
            while (_pollerRunning)
            {
                var next = _consumer.Consume().Message;
                if (next == null) continue;
                
                _toProcess.Enqueue(next);
            }
            _consumer.Close();
    }
    void Start()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = SystemInfo.deviceUniqueIdentifier,
            FetchMinBytes=64,
            FetchWaitMaxMs = 300
        };

        var cb = new ConsumerBuilder<int, Vector3>(config);
        cb.SetValueDeserializer(new Vector3Deserializer());
        _consumer = cb.Build();

        _consumer.Subscribe("player-pos");

        _poller = new Thread(PollerLoop);
        _poller.Start();

    }

    private void LateUpdate()
    {
        while (_toProcess.Count != 0)
        {
            Message<int, Vector3> curr;
            if (!_toProcess.TryDequeue(out curr)) break;
             
            if (!_playerPositionSubscribers.ContainsKey(curr.Key))
                _playerPositionSubscribers.Add(curr.Key, Instantiate(_subscriberPrefab, curr.Value, Quaternion.identity));
            else
            {
                PlayerPositionSubscriber pps = _playerPositionSubscribers[curr.Key];
                pps.transform.position = curr.Value;
            }
        }
    }

    private void OnDestroy()
    {
        _pollerRunning = false;
    }
}
