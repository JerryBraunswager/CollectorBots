using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider _pickCollider;
    [SerializeField] private Base _base;

    private bool _isBusy = false;
    private bool _isBusy1 = false;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Resource _picked = null;

    public bool IsBusy => _isBusy;
    public Base Home => _base;

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
        if (_isBusy == false || _isBusy1 == true)
        {
            return;
        }

        if (transform.childCount == 1 & other.TryGetComponent(out Resource resource) & _base.CheckScanList(resource))
        {
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            other.isTrigger = false;
            _picked = resource;
            resource.Pick();
            _target = _base.transform;
        }
    }

    public void Init(Base home)
    {
        _base = home;
        _isBusy1 = false;
    }

    public void ProvideResource(Resource checkedResource)
    {
        _isBusy = true;
        _picked = checkedResource;
        _target = _picked.transform;
    }

    public void ProvideTarget(Transform target) 
    {
        _isBusy = true;
        _isBusy1 = true;
        _picked = null;
        _target = target;
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
        _picked.Deliver();
        _picked = null;
        return resource;
    }
}
