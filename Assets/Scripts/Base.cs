using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _leftDownPointSpawn;
    [SerializeField] private Transform _rightUpPointSpawn;
    [SerializeField] private float _unitPriece;
    [SerializeField] private float _basePrice;
    [SerializeField] private int _startCount;
    [SerializeField] private FlagPlacer _flag;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Scaner _scaner;

    private Resource _scannedResource;
    private float _score = 0;
    private Unit _freeUnit;

    public event Action<float> ScoreChanged;
    public event Action<Base> BaseChoosed;

    public int UnitCount => _unitSpawner.UnitCount;

    private void Start()
    {
        ScoreChanged?.Invoke(_score);

        for (int i = 0; i < _startCount; i++) 
        {
            SpawnUnit();
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _unitSpawner.GetUnits())
        {
            unit.PickedUp -= InsertPickPool;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            if (unit.Home == transform)
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
        if(_flag == null)
        {
            return;
        }

        _freeUnit = _unitSpawner.FreeUnit(transform);

        ProvideResource();

        if (IsChoosed())
        {
            if (_score >= _basePrice)
            {
                _flag.Init(_freeUnit);
                _score -= _basePrice;
                ScoreChanged?.Invoke(_score);
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
        if(_unitSpawner.UnitCount <= 1 )
        {
            return;
        }

        BaseChoosed?.Invoke(this);
    }

    private bool IsChoosed()
    {
        if (_flag == null)
        {
            return false;
        }

        return _flag.ChoosedBase == this;
    }

    public void Init(FlagPlacer flagPlacer, ResourceFactory resorces, UnitSpawner unitSpawner, Scaner scaner)
    {
        _flag = flagPlacer;
        _unitSpawner = unitSpawner;
        _scaner = scaner;
        _resourceSpawner.SetPool(resorces);
    }

    public void InsertPickPool(Resource resource)
    {
        _scaner.PickResource(resource);
    }

    private void ProvideResource()
    {
        if (_scannedResource == null)
        {
            _scannedResource = _scaner.ChooseResource(transform);
        }

        Unit unit = _freeUnit;

        if (unit == null || _scannedResource == null)
        {
            return;
        }

        unit.ProvideTarget(_scannedResource.transform);
        _scannedResource = null;
    }

    private void SpawnUnit()
    {
        Unit spawned = _unitSpawner.SpawnUnit(this, _leftDownPointSpawn, _rightUpPointSpawn);
        spawned.PickedUp += InsertPickPool;
    }
}
