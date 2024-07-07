using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    [SerializeField]private float _score;

    private bool _isSelected = false;
    private bool _isPickedUp = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool IsSelected => _isSelected;
    public bool IsPickedUp => _isPickedUp;
    public float Score => _score;

    public event Action<Resource> Picked;

    public void PickedUp()
    {
        Picked.Invoke(this);
        _isSelected = false;
        _isPickedUp = false;
        _rigidbody.isKinematic = false;
    }

    public void Select()
    {
        _isSelected = true;
    }

    public void Pick()
    {
        _isPickedUp = true;
    }
}
