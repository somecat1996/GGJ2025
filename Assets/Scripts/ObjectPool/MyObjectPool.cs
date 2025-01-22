using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I use the old script from last assignment.
/// This script is the single object pool.
/// </summary>
/// <author>Lin Wang</author>
/// <date>October 21, 2024</date>
/// <version>1.0</version>

public class MyObjectPool : MonoBehaviour
{
    GameObject prefab;
    Stack<GameObject> objectStack;

    /// <summary>
    /// Initialize object pool
    /// </summary>
    /// <param name="p">Pooled object</param>
    /// <param name="initNumber">Initial object number</param>
    public void Init(GameObject p, int initNumber = 0)
    {
        prefab = p;
        objectStack = new Stack<GameObject>();

        for (int i = 0; i < initNumber; i++)
        {
            GameObject o = Instantiate(prefab, transform);
            o.SetActive(false);
            objectStack.Push(o);
        }
    }

    /// <summary>
    /// Get one available object from pool.
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        if (objectStack.Count <= 0)
        {
            GameObject o = Instantiate(prefab, transform);
            o.SetActive(false);
            return o;
        }
        else
        {
            return objectStack.Pop();
        }
    }

    /// <summary>
    /// Release one object.
    /// </summary>
    /// <param name="o">Object to be released</param>
    public void Release(GameObject o)
    {
        o.SetActive(false);
        objectStack.Push(o);
    }
}
