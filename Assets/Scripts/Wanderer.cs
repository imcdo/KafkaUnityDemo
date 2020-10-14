using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{

    private Vector3 _startPos;
    private Vector3 _prev;
    [SerializeField] private float _randWeight = 20;
    [SerializeField] private float _dirWeight = 20;
    [SerializeField] private float _speed = 5;

    private Vector3 _rand;
    void Start()
    {
        _startPos = transform.position;
        _prev = _startPos;
        _rand = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;

        StartCoroutine(ResetRand());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 originGravity = _startPos - transform.position;
        Vector3 currDir = transform.position - _prev;
        
        
        transform.position +=
            (currDir.normalized * _dirWeight + originGravity + _rand * _randWeight).normalized * _speed * Time.deltaTime;
        
        _prev = transform.position;
    }

    IEnumerator ResetRand()
    {
        while (true)
        {
            _rand = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            yield return new WaitForSeconds(3);
        }
    }
}
