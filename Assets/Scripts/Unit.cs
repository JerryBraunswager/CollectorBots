using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider _pickCollider;
    [SerializeField] private Base _base;

    private bool _isBusy = false;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Resource _picked = null;

    public bool IsBusy => _isBusy;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_isBusy == false) 
        {
            return;
        }

        Vector3 movementVector = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(movementVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isBusy == false)
        {
            return;
        }

        if (transform.childCount == 1 && other.TryGetComponent(out Resource resource) && _base.CheckScanList(resource))
        {
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            _picked = resource;
            _target = _base.transform;
        }
    }

    public void ProvideResource(Resource checkedResource)
    {
        _isBusy = true;
        _picked = checkedResource;
        _target = _picked.transform;
    }

    public Resource Deliver()
    {
        _isBusy = false;

        if(_picked == null)
        {
            return null;
        }
        Resource resource = _picked;
        _picked.Receive();
        _picked = null;
        _target = null;
        return resource;
    }
}
