using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class ResourceFactory : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private Resource resource;

    private ObjectPool<Resource> _freeResources = new ObjectPool<Resource>();
    private ObjectPool<Resource> _pickedResources = new ObjectPool<Resource>();
    private ObjectPool<Resource> _choosedResources = new ObjectPool<Resource>();

    public ReadOnlyCollection<Resource> FreeResources => _freeResources.PooledObjects.ToList().AsReadOnly();

    private void OnEnable()
    {
        _scaner.Choosed += ChooseResource;
        _scaner.Picked += PickResource;
        _scaner.Deleted += DeleteResource;
    }

    private void OnDisable()
    {
        _scaner.Choosed -= ChooseResource;
        _scaner.Picked -= PickResource;
        _scaner.Deleted -= DeleteResource;
    }

    public void ChooseResource(Resource resource)
    {
        _freeResources.RemoveObject(resource);
        _choosedResources.PutObject(resource);
    }

    public void PickResource(Resource resource)
    {
        _choosedResources.RemoveObject(resource);
        _pickedResources.PutObject(resource);
    }

    public void DeleteResource(Resource resource) 
    {
        _pickedResources.RemoveObject(resource);
        resource.gameObject.SetActive(false);
        resource.transform.parent = transform;
    }

    public Resource GetResourceFromPool()
    {
        Resource result = _freeResources.GetObject();

        if (result == null)
        {
            result = Instantiate(resource);
            result.gameObject.SetActive(false);
            result.transform.parent = transform;
            _freeResources.PutObject(result);
        }

        return result;
    }
}
