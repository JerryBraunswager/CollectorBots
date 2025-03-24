using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    [SerializeField]private float _worth;

    private Rigidbody _rigidbody;
    private Collider _collider;

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
}
