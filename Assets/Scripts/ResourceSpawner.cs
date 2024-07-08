using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResorcesPool))]
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private Vector2 _rectangle;

    private ResorcesPool _pool;

    private void Awake()
    {
        _pool = GetComponent<ResorcesPool>();
    }

    private void OnDisable()
    {
        for(int i = 0; i < _pool.PooledObjects.Count(); i++) 
        {
            _pool.PooledObjects.ElementAt(i).Received -= ReturnResourceInPool;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnResources(new WaitForSeconds(_timeToSpawn)));
    }

    private IEnumerator SpawnResources(WaitForSeconds sleepTime)
    {
        while (enabled)
        {
            yield return sleepTime;
            Resource resource = _pool.GetObject();
            float positionX = Random.Range(-(_rectangle.x), _rectangle.x);
            float positionY = Random.Range(-(_rectangle.y), _rectangle.y);
            resource.transform.position = new Vector3(positionX, 0, positionY);
            resource.transform.gameObject.SetActive(true);
            resource.Received += ReturnResourceInPool;
        }
    }

    private void ReturnResourceInPool(Resource resource)
    {
        _pool.PutObject(resource);
        resource.Received -= ReturnResourceInPool;
    }
}
