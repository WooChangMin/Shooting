using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTester : MonoBehaviour
{
    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Poolable poolable = objectPool.Get();
            poolable.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }
    }
}
