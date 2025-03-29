using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Scaner : MonoBehaviour
{
    [SerializeField] private float _sleepTime;
    [SerializeField] private LayerMask _layerMask;

    private Vector3 _size = new Vector3(20, 0, 9);
    private List<Collider> _colliders = new List<Collider>();
    private List<Collider> _choosed = new List<Collider>();

    public event UnityAction<Resource> Choosed;
    public event UnityAction<Resource> Picked;
    public event UnityAction<Resource> Deleted;

    private void Update()
    {
        if(_colliders == null && _colliders.Count() > 0)
        {
            return;
        }

        _colliders = Physics.OverlapBox(Vector3.zero, _size, Quaternion.identity, _layerMask.value).ToList();
    }

    public Resource ChooseResource(Transform unitBase)
    {
        if (_colliders == null)
        {
            return null;
        }

        var overlaps = _colliders.OrderBy((collider) => (collider.transform.transform.position - unitBase.position).sqrMagnitude).ToArray();

        overlaps = overlaps.Except(_choosed).ToArray();
        Resource result = null;

        foreach (Collider collider in overlaps)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                if(collider.transform.parent != transform)
                {
                    continue;
                }

                result = resource;
                Choosed?.Invoke(result);
                _colliders.Remove(collider);
                _choosed.Add(collider);
                break;
            }
        }

        return result;
    }

    public void RemoveResource(Resource resource)
    {
        if (resource.TryGetComponent(out Collider collider))
        {
            _choosed.Remove(collider);
        }

        Deleted?.Invoke(resource);
    }

    public void PickResource(Resource resource)
    {
        Picked?.Invoke(resource);
    }
}
