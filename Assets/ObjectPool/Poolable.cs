using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField] float releaseTime;

    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set { pool = value; } }

    private void OnEnable()
    {
        StartCoroutine(ReleaseTimer());
    }

    IEnumerator ReleaseTimer()
    {
        yield return new WaitForSeconds(releaseTime);
        pool.Release(this);
    }

}
