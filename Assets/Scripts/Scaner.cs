using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Vector3 _size = new Vector3(18, 0, 7);
    private List<Resource> _scannedResources = new List<Resource>();

    public Resource Scan()
    {
        Collider[] overlaps = Physics.OverlapBox(Vector3.zero, _size, Quaternion.identity, _layerMask.value);
        overlaps = overlaps.OrderBy((collider) => (collider.transform.transform.position - transform.position).sqrMagnitude).ToArray();
        Resource returnedObject = null;

        foreach (Collider collider in overlaps)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                if (_scannedResources.Contains(resource) == false)
                {
                    returnedObject = resource;
                    _scannedResources.Add(resource);
                    break;
                }
            }
        }

        return returnedObject;
    }

    public void RemoveResource(Resource resource)
    {
        _scannedResources.Remove(resource);
    }

    public bool IsChecked(Resource resource)
    {
        return _scannedResources.Contains(resource);
    }
}
