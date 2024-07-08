using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    [SerializeField]private float _worth;

    private Rigidbody _rigidbody;

    public event Action<Resource> Received;
    public float Worth => _worth;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Receive()
    {
        Received.Invoke(this);
        _rigidbody.isKinematic = false;
    }
}
