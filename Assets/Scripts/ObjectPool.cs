using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private List<T> _pool = new List<T>();

    public IEnumerable<T> PooledObjects => _pool;

    public T GetObject()
    {
        foreach (T obj in _pool) 
        { 
            if(obj.gameObject.activeSelf)
            {
                return obj;
            }
        }

        return null;
    }

    public void RemoveObject(T obj)
    {
        _pool.Remove(obj);
    }

    public void PutObject(T obj)
    {
        _pool.Add(obj);
    }
}
