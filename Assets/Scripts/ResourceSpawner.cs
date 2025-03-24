using System.Collections;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private Vector2 _rectangle;

    [SerializeField] private Resorces _pool;

    private void OnDisable()
    {
        for(int i = 0; i < _pool.AllResources.Count(); i++) 
        {
            _pool.AllResources.ElementAt(i).Received -= ReturnResourceInPool;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnResources(new WaitForSeconds(_timeToSpawn)));
    }

    public void SetPool(Resorces pool)
    {
        _pool = pool;
    }

    private IEnumerator SpawnResources(WaitForSeconds sleepTime)
    {
        while (enabled)
        {
            yield return sleepTime;
            Resource resource = _pool.GetResourceFromPool();
            resource.gameObject.SetActive(true);
            float positionX = Random.Range(-(_rectangle.x), _rectangle.x);
            float positionY = Random.Range(-(_rectangle.y), _rectangle.y);
            resource.transform.position = new Vector3(positionX, 0, positionY);
            resource.transform.gameObject.SetActive(true);
            resource.Received += ReturnResourceInPool;
        }
    }

    private void ReturnResourceInPool(Resource resource)
    {
        _pool.DeleteResource(resource);
        resource.Received -= ReturnResourceInPool;
    }
}
