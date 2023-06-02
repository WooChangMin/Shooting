using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] Poolable poolablePrefab;

        [SerializeField] int poolSize;
        [SerializeField] int maxSize;

        private Stack<Poolable> objectPool = new Stack<Poolable>();

        private void Awake()
        {
            CreatePool();
        }

        public void CreatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.gameObject.SetActive(false);
                poolable.Pool = this;
                poolable.transform.SetParent(transform);           //폴더링처럼 프리팹들을 모아놓기 위함
                objectPool.Push(poolable);
            }
        }

        public Poolable Get()
        {

            if (objectPool.Count > 0)
            {
                Poolable poolable = objectPool.Pop();
                poolable.gameObject.SetActive(true);
                poolable.transform.parent = null;
                return poolable;
            }
            else
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.Pool = this;
                return poolable;
            }
        }

        public void Release(Poolable poolable)
        {
            if (objectPool.Count < maxSize)
            {
                poolable.gameObject.SetActive(false);
                poolable.transform.SetParent(transform);
                objectPool.Push(poolable);
            }
            else
            {
                Destroy(poolable.gameObject);
            }
        }
    }
}