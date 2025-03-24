using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flag : MonoBehaviour
{
    [SerializeField] private GameObject[] _flagProp;

    private Collider _collider;

    public event Action<Unit> SteppedUp;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            SteppedUp?.Invoke(unit);
        }
    }

    public void ChangeState(bool state)
    {
        foreach(var prop in _flagProp) 
        { 
            prop.SetActive(state);
            _collider.enabled = state;
        }
    }
}
