using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Scaner))]
public class Base : MonoBehaviour
{
    [SerializeField] private float _unitPriece;
    [SerializeField] private float _basePrice;
    [SerializeField] private Transform _leftDownPointSpawn;
    [SerializeField] private Transform _rightUpPointSpawn;
    [SerializeField] private Unit _unit;
    [SerializeField] private int _startCount;

    private Scaner _scaner;
    private Flag _flag;
    private List<Unit> _units = new List<Unit>();
    private float _score = 0;

    public Flag Flag => _flag;

    public event Action<float> ScoreChanged;
    public event Action<Base> BaseChoosed;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
    }

    private void Start()
    {
        ScoreChanged?.Invoke(_score);

        for (int i = 0; i < _startCount; i++) 
        {
            SpawnUnit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            if (unit.Home == this)
            {
                Resource received = unit.Deliver();

                if (received == null)
                {
                    return;
                }
                _score += received.Worth;
                ScoreChanged?.Invoke(_score);
                _scaner.RemoveResource(received);
            }
        }
    }

    private void Update()
    {
        ProvideResource();

        if (CheckFlag())
        {
            if (_score >= _basePrice)
            {
                foreach (Unit unit in _units)
                {
                    if (!unit.IsBusy)
                    {
                        _flag.Init(unit, this, transform.parent.GetComponent<Ground>());
                        _score -= _basePrice;
                        ScoreChanged?.Invoke(_score);
                        break;
                    }
                }
            }
        }
        else
        {
            if (_score >= _unitPriece)
            {
                _score -= _unitPriece;
                ScoreChanged?.Invoke(_score);
                SpawnUnit();
            }
        }
    }

    private void OnMouseDown()
    {
        BaseChoosed?.Invoke(this);
    }

    public bool CheckFlag()
    {
        return _flag != null;
    }

    public void InitFlag(Flag flag)
    {
        if(_units.Count > 1) 
        { 
            _flag = flag;
        }
        else
        {
            Destroy(flag.gameObject);
        }
    }

    public bool CheckScanList(Resource resource)
    {
        return _scaner.IsChecked(resource);
    }

    private void SpawnUnit()
    {
        Unit spawned = Instantiate(_unit);
        float x = Random.Range(_leftDownPointSpawn.position.x, _rightUpPointSpawn.position.x);
        float z = Random.Range(_leftDownPointSpawn.position.z, _rightUpPointSpawn.position.z);
        spawned.transform.position = new Vector3(x, 1, z);
        spawned.Init(this);
        _units.Add(spawned);
    }

    private void ProvideResource()
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
}
