using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;  //오브젝트 풀을 여러개 만들지않고 특정별로 분류해서 찾을수 있게 Dic형식 사용


    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, prefab);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object  // Object는 유니티에서 사용하는 가장 기본클래스 -> gameObject와 컴포넌트가 상속받았으므로 Object자리에 컴포넌트 or 게임오브젝트가 들어갈 수 있음
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;                  // 형변환 필요

            if (poolDic.ContainsKey(prefab.name))
                CreatePool(prefab.name, prefab);

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();                                  // 유니티 pool은 없을떄 만들어줌
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T;                                              // 여기도 형변환
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject go = poolDic[key].Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go.GetComponent<T>();

        }
        else
        {
            return null;
        }
    }

    /*public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)                      //위쪽에서 동일한 get 제네릭으로 구현 
    {
        if (poolDic.ContainsKey(prefab.name))
            CreatePool(prefab.name, prefab);

        ObjectPool<GameObject> pool = poolDic[prefab.name];
        GameObject go = pool.Get();                                  // 유니티 pool은 없을떄 만들어줌
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }*/


    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity);  
    }

    public bool Release(GameObject go)                      // 반납여부 true /false반환
    {
        if (!poolDic.ContainsKey(go.name))                  // 반납하려고 했는데 없을때
            return false;
        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go);                                   // 유니티 pool은 꽉차있을때 반납하지않고 자동삭제해줌
        return true;
    }

    private void CreatePool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject go = Instantiate(prefab);
                go.name = key;  
                return go;
            },
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                go.transform.SetParent(null);
            },
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);
            },
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }
            );
        poolDic.Add(key, pool); 
    }
    public bool IsContain<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

}
