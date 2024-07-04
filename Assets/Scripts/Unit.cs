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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_target != _base.transform)
        {
            return;
        }

        if(transform.childCount == 1)
        {
            _target = null;
            _isBusy = false;
        }
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

        if (other.TryGetComponent(out Resource resource))
        {
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            _target = _base.transform;
        }
    }

    public bool IsBusy => _isBusy;

    public void ProvideResource(Resource checkedResource)
    {
        checkedResource.Checked();
        _isBusy = true;
        _target = checkedResource.transform;
    }
}
