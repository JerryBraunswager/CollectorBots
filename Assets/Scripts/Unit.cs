using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider _pickCollider;
    [SerializeField] private Base _base;

    private bool _isBusy = false;
    private bool _isPickUp = false;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Resource _picked = null;

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

        //if(transform.childCount == 1)
        //{
        //    _target = null;
        //    _isBusy = false;
        //    _isPickUp = false;
        //}
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

        if (_isPickUp == false && other.TryGetComponent(out Resource resource) && !resource.IsPickedUp)
        {
            other.transform.SetParent(transform);
            other.attachedRigidbody.isKinematic = true;
            _picked = resource;
            resource.Pick();
            _isPickUp = true;
            _target = _base.transform;
        }
    }

    public bool IsBusy => _isBusy;

    public void ProvideResource(Resource checkedResource)
    {
        checkedResource.Select();
        _isBusy = true;
        _picked = checkedResource;
        _target = _picked.transform;
    }

    public float Deliver()
    {
        if(_picked == null)
        { 
            return 0; 
        }

        float score = _picked.Score;
        _picked.PickedUp();
        _picked = null;
        _target = null;
        _isBusy = false;
        _isPickUp = false;
        return score;
    }
}
