using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider _pickCollider;
    [SerializeField] private Transform _base;

    private bool _isBusy = false;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Resource _picked = null;
    private Transform _parent;

    public bool IsBusy => _isBusy;
    public Transform Home => _base;

    public event Action<Resource> PickedUp;
    public event Action<Unit> ReachedFlag;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_isBusy == false || _target == null) 
        {
            return;
        }

        Vector3 movementVector = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(movementVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickResource(other);

        if (other.TryGetComponent(out Flag flag))
        {
            ReachedFlag?.Invoke(this);
        }
    }

    public void Init(Transform home)
    {
        _base = home;
    }

    public void ProvideTarget(Transform target) 
    {
        _isBusy = true;
        _picked = null;
        _target = target;
        _parent = target.parent;
    }

    public Resource Deliver()
    {
        _isBusy = false;
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

    private void PickResource(Collider other)
    {
        if(_target == null || other.isTrigger == false)
        {
            return;
        }

        if (other.transform == _target.transform & transform.childCount == 1 & other.TryGetComponent(out Resource resource))
        {
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = false;
            _picked = resource;
            _target = _base;
            PickedUp?.Invoke(resource);
        }
    }
}
