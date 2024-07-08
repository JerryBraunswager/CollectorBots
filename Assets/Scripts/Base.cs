using System;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
public class Base : MonoBehaviour
{
    [SerializeField] private Unit[] _units;

    private Scaner _scaner;
    private float _score = 0;

    public event Action<float> ScoreChanged;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
    }

    private void Start()
    {
        ScoreChanged.Invoke(_score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            Resource received = unit.Deliver();

            if (received == null)
            {
                return;
            }

            _score += received.Worth;
            _scaner.RemoveResource(received);
            ScoreChanged.Invoke(_score);
        }
    }

    private void Update()
    {
        Resource resource = _scaner.Scan();

        if (resource == null)
        {
            return;
        }

        foreach (Unit unit in _units)
        {
            if (!unit.IsBusy)
            {
                unit.ProvideResource(resource);
                break;
            }
        }
        
    }

    public bool CheckScanList(Resource resource)
    {
        return _scaner.IsChecked(resource);
    }
}
