using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
[RequireComponent (typeof(UnitSpawner))]
public class Base : MonoBehaviour
{
    [SerializeField] private float _unitPriece;
    [SerializeField] private float _basePrice;
    [SerializeField] private int _startCount;
    [SerializeField] private FlagPlacer _flag;
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private Scaner _scaner;
    private UnitSpawner _unitSpawner;
    private Resource _scannedResource;
    private float _score = 0;

    public int UnitCount => _unitSpawner.UnitCount;

    public event Action<float> ScoreChanged;
    public event Action<Base> BaseChoosed;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void Start()
    {
        ScoreChanged?.Invoke(_score);

        for (int i = 0; i < _startCount; i++) 
        {
            _unitSpawner.SpawnUnit();
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
        if(_flag == null)
        {
            return;
        }

        ProvideResource();

        if (CheckFlag())
        {
            if (_score >= _basePrice)
            {
                _flag.Init(_unitSpawner.FreeUnit());
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
                _unitSpawner.SpawnUnit();
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

    public bool CheckFlag()
    {
        if (_flag == null)
        {
            return false;
        }

        return _flag.ChoosedBase == this;
    }

    public void Init(FlagPlacer flagPlacer, Resorces resorces)
    {
        _flag = flagPlacer;
        _scaner.Init(resorces);
        _resourceSpawner.SetPool(resorces);
    }

    public void InsertPickPool(Resource resource)
    {
        _scaner.PickResource(resource);
    }

    public void AddUnit(Unit unit) 
    {
        _unitSpawner.AddUnit(unit);
    }

    public void RemoveUnit(Unit unit) 
    { 
        _unitSpawner.RemoveUnit(unit);
    }

    private void ProvideResource()
    {
        if (_scannedResource == null)
        {
            _scannedResource = _scaner.Scan();
        }

        Unit unit = _unitSpawner.FreeUnit();

        if (unit == null || _scannedResource == null)
        {
            return;
        }

        unit.ProvideResource(_scannedResource);
        _scannedResource = null;
    }
}
