using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ResorcesPool))]
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private Vector2 _rectangle;

    private WaitForSeconds _sleepTime;
    private ResorcesPool _pool;

    private void Awake()
    {
        _pool = GetComponent<ResorcesPool>();
    }

    private void Start()
    {
        _sleepTime = new WaitForSeconds(_timeToSpawn);
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        while (enabled)
        {
            yield return _sleepTime;
            Resource resource = _pool.GetObject();
            resource.transform.position = new Vector3(Random.Range(-(_rectangle.x), _rectangle.x), 0, Random.Range(-(_rectangle.y), _rectangle.y));
            resource.transform.gameObject.SetActive(true);
            resource.Picked += ResourceObserve;
        }
    }

    private void ResourceObserve(Resource resource)
    {
        _pool.PutObject(resource);
        resource.Picked -= ResourceObserve;
    }
}
