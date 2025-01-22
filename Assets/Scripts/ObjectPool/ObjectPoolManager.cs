using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I use the old script from last assignment.
/// This script is manager of all object pools.
/// </summary>
/// <author>Lin Wang</author>
/// <date>October 21, 2024</date>
/// <version>1.0</version>

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    // The prefab of object pool.
    public GameObject objectPool;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Get an object from object pool. If this object does not have an object pool, a new pool will be created.
    /// </summary>
    /// <param name="prefab">The prefab of this object.</param>
    /// <returns></returns>
    public GameObject Get(GameObject prefab)
    {
        string name = prefab.name.Split("(")[0];
        MyObjectPool myObjectPool;
        Transform pool = transform.Find(name + "_Pool");
        if (pool == null)
        {
            GameObject tmpObjectPool = Instantiate(objectPool, transform);
            tmpObjectPool.name = name + "_Pool";
            myObjectPool = tmpObjectPool.GetComponent<MyObjectPool>();
            myObjectPool.Init(prefab, 3);
        }
        else
        {
            myObjectPool = pool.GetComponent<MyObjectPool>();
        }
        return myObjectPool.Get();
    }

    /// <summary>
    /// Release an object back to its object pool.
    /// </summary>
    /// <param name="o">The object to be released</param>
    public void Release(GameObject o)
    {
        string name = o.name.Split("(")[0];
        MyObjectPool myObjectPool;
        Transform pool = transform.Find(name + "_Pool");
        if (pool == null)
        {
            GameObject tmpObjectPool = Instantiate(objectPool, transform);
            tmpObjectPool.name = name + "_Pool";
            myObjectPool = tmpObjectPool.GetComponent<MyObjectPool>();
            myObjectPool.Init(o, 3);
        }
        else
        {
            myObjectPool = pool.GetComponent<MyObjectPool>();
        }
        myObjectPool.Release(o);
    }
}
