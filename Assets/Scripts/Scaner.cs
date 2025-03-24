using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Resorces _pool;

    private Vector3 _size = new Vector3(20, 0, 9);

    public Resource Scan()
    {
        //Collider[] overlaps = Physics.OverlapBox(Vector3.zero, _size, Quaternion.identity, _layerMask.value);
        //overlaps = overlaps.OrderBy((collider) => (collider.transform.transform.position - transform.position).sqrMagnitude).ToArray();
        //Resource returnedObject = /*_pool.GetResourceFromPool();*/null;
        //_pool.ChooseResource(returnedObject);

        //foreach (Collider collider in overlaps)
        //{
        //    if (collider.TryGetComponent(out Resource resource))
        //    {
        //        if (_pool.GetResourceFromPool)
        //        {
        //            returnedObject = resource;
        //            _pool.ChooseResource(resource);
        //            break;
        //        }
        //    }
        //}

        return _pool.ChooseResource();//returnedObject;
    }

    public void Init(Resorces resources)
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
