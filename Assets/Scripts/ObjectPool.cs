using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private Queue<T> _pool;

    public IEnumerable<T> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<T>();
    }

    public T GetObject()
    {
        if (_pool.Count == 0)
        {
            var obj = Instantiate(_prefab);
            obj.transform.parent = transform;

            return obj;
        }

        return _pool.Dequeue();
    }

    public void PutObject(T obj)
    {
        obj.transform.parent = transform;
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public void Reset()
    {
        _pool.Clear();
    }
}
