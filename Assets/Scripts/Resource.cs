using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    [SerializeField]private float _score;

    private bool _isChecked = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        Picked.Invoke(this);
    }

    public bool IsChecked => _isChecked;
    public float Score => _score;

    public event Action<Resource> Picked;

    public void PickedUp()
    {
        Picked.Invoke(this);
        _isChecked = false;
        _rigidbody.isKinematic = false;
    }

    public void Checked()
    {
        _isChecked = true;
    }
}
