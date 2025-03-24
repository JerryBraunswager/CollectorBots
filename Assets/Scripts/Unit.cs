using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider _pickCollider;
    [SerializeField] private Base _base;

    private bool _isBusyResource = false;
    private bool _isBusyTarget = false;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Resource _picked = null;

    public bool IsBusy => _isBusyResource;
    public Base Home => _base;

    public event Action<Resource> PickedUp;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_isBusyResource == false || _target == null) 
        {
            return;
        }

        Vector3 movementVector = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(movementVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_picked == null || _isBusyResource == false || _isBusyTarget == true)
        {
            return;
        }
        
        if (transform.childCount == 1 & other.TryGetComponent(out Resource resource))
        {
            if(resource != _picked)
            {
                return;
            }
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = false;
            _target = _base.transform;
            PickedUp?.Invoke(resource);
        }
    }

    public void Init(Base home)
    {
        if (_base != null)
        {
            _base.RemoveUnit(this);
        }
        _base = home;
        _base.AddUnit(this);
        _isBusyTarget = false;
    }

    public void ProvideResource(Resource checkedResource)
    {
        _isBusyResource = true;
        _picked = checkedResource;
        _target = _picked.transform;
    }

    public void ProvideTarget(Transform target) 
    {
        _isBusyResource = true;
        _isBusyTarget = true;
        _picked = null;
        _target = target;
    }

    public Resource Deliver()
    {
        _isBusyResource = false;
        _target = null;

        if(_picked == null)
        {
            return null;
        }

        Resource resource = _picked;
        _picked.Receive();
        _picked = null;
        return resource;
    }
}
