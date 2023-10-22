using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class MovementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefabObject;
    [SerializeField] private int _numberOfObjects;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _waitTime;

    private Transform[] _objectsInScene;
    private TransformAccessArray _transformAccessArray;

    private void Start()
    {
        _objectsInScene = new Transform[_numberOfObjects];
        for (int i = 0; i < _numberOfObjects; i++)
        {
            Vector3 position = new Vector3(Random.Range(-100, 100), 5.5f, Random.Range(-100, 100));
            GameObject instance = Instantiate(_prefabObject, position, Quaternion.identity);
            _objectsInScene[i] = instance.transform;
        }
        _transformAccessArray = new TransformAccessArray(_objectsInScene);
        StartCoroutine(LogRoutine());
    }

    private void Update()
    {
        MovementJob movementJob = new MovementJob()
        {
            Speed = _moveSpeed,
        };
        JobHandle movementJobHandle = movementJob.Schedule(_transformAccessArray);
        movementJobHandle.Complete();
    }

    private void OnDestroy()
    {
        _transformAccessArray.Dispose();
    }

    private IEnumerator LogRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitTime);

            NativeArray<int> numbers = new NativeArray<int>(_objectsInScene.Length, Allocator.Persistent);
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = Random.Range(1, 100);

            LogJob logJob = new LogJob()
            {
                RndNum = numbers
            };

            JobHandle logJobHandle = logJob.Schedule();
            logJobHandle.Complete();

            numbers.Dispose();
        }
    }
}
