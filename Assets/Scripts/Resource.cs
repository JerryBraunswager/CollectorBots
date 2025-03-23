using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    [SerializeField]private float _worth;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _isPicked = false;

    public bool isPicked => _isPicked;

    public event Action<Resource> Received;
    public float Worth => _worth;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Receive()
    {
        Received?.Invoke(this);
        _rigidbody.isKinematic = false;
        _collider.isTrigger = true;
    }

    public void Pick()
    {
        _isPicked = true;
    }

    public void Deliver()
    {
        _isPicked = false;
    }
}
