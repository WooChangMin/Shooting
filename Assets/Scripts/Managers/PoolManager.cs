using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;  //������Ʈ Ǯ�� ������ �������ʰ� Ư������ �з��ؼ� ã���� �ְ� Dic���� ���


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
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object  // Object�� ����Ƽ���� ����ϴ� ���� �⺻Ŭ���� -> gameObject�� ������Ʈ�� ��ӹ޾����Ƿ� Object�ڸ��� ������Ʈ or ���ӿ�����Ʈ�� �� �� ����
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;                  // ����ȯ �ʿ�

            if (poolDic.ContainsKey(prefab.name))
                CreatePool(prefab.name, prefab);

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();                                  // ����Ƽ pool�� ������ �������
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T;                                              // ���⵵ ����ȯ
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

    /*public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)                      //���ʿ��� ������ get ���׸����� ���� 
    {
        if (poolDic.ContainsKey(prefab.name))
            CreatePool(prefab.name, prefab);

        ObjectPool<GameObject> pool = poolDic[prefab.name];
        GameObject go = pool.Get();                                  // ����Ƽ pool�� ������ �������
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }*/


    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity);  
    }

    public bool Release(GameObject go)                      // �ݳ����� true /false��ȯ
    {
        if (!poolDic.ContainsKey(go.name))                  // �ݳ��Ϸ��� �ߴµ� ������
            return false;
        ObjectPool<GameObject> pool = poolDic[go.name];
        pool.Release(go);                                   // ����Ƽ pool�� ���������� �ݳ������ʰ� �ڵ���������
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
