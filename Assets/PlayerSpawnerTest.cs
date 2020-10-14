using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerTest : MonoBehaviour
{
    [SerializeField] private PlayerPositionProducer _producerPrefab;
    private int _count = 0;

    public void SpawnPlayer()
    {
        var prod = Instantiate(_producerPrefab,Random.insideUnitSphere * 40, Quaternion.identity);
        prod.PlayerNumber = _count;

        _count++;
    }
}
