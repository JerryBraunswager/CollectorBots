using System.Linq;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private ResourceFactory _pool;

    private Vector3 _size = new Vector3(20, 0, 9);

    public Resource Scan()
    {
        Collider[] overlaps = Physics.OverlapBox(Vector3.zero, _size, Quaternion.identity, _layerMask.value);
        overlaps = overlaps.OrderBy((collider) => (collider.transform.transform.position - transform.position).sqrMagnitude).ToArray();
        Resource returnedObject = null;

        foreach (Collider collider in overlaps)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                if (_pool.ChooseResource(resource))
                {
                    returnedObject = resource;
                    break;
                }
            }
        }

        return returnedObject;
    }

    public void Init(ResourceFactory resources)
    {
        _pool = resources;
    }

    public void RemoveResource(Resource resource)
    {
        _pool.DeleteResource(resource);
    }

    public void PickResource(Resource resource)
    {
        _pool.PickResource(resource);
    }
}
