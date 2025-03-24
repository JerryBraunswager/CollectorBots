using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private List<T> _pool = new List<T>();

    public IEnumerable<T> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new List<T>();
    }

    public T GetObject()
    {
        foreach (T obj in _pool) 
        { 
            if(obj.gameObject.activeSelf == false)
            {
                //_pool.Remove(obj);
                return obj;
            }
        }

        var newObj = Instantiate(_prefab);
        newObj.transform.parent = transform;
        _pool.Add(newObj);
        return newObj;
    }

    public T PeakObject()
    {
        foreach (T obj in _pool)
        {
            if (obj.gameObject.activeSelf == true)
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
