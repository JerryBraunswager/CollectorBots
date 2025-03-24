using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Resorces : MonoBehaviour
{
    [SerializeField] private ObjectPool<Resource> _allResources;
    [SerializeField] private ObjectPool<Resource> _pickedResources;

    private List<Resource> _choosedResources = new List<Resource>();

    public ReadOnlyCollection<Resource> AllResources => _allResources.PooledObjects.ToList().AsReadOnly();
    public ReadOnlyCollection<Resource> ChoosedResources => _pickedResources.PooledObjects.ToList().AsReadOnly();

    public Resource ChooseResource()
    {
        Resource resource = _allResources.PeakObject();

        if (resource == null)
        { 
            return null; 
        }
           
        _allResources.RemoveObject(resource);
        _choosedResources.Add(resource);
        return resource;
    }

    public void PickResource(Resource resource)
    {
        _choosedResources.Remove(resource);
        _pickedResources.PutObject(resource);
    }

    public void DeleteResource(Resource resource) 
    {
        _pickedResources.RemoveObject(resource);
        resource.gameObject.SetActive(false);
        resource.transform.parent = transform;

        if(_allResources.PooledObjects.Contains(resource))
        {
            return;
        }

        _allResources.PutObject(resource);
    }

    public Resource GetResourceFromPool()
    {
        return _allResources.GetObject();
    }
}
