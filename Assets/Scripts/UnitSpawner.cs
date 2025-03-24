using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

[RequireComponent(typeof(Base))]
public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Transform _leftDownPointSpawn;
    [SerializeField] private Transform _rightUpPointSpawn;
    [SerializeField] private Unit _unit;

    private Base _base;
    private List<Unit> _units = new List<Unit>();

    public int UnitCount => _units.Count;

    private void Awake()
    {
        _base = GetComponent<Base>();
    }

    public void SpawnUnit()
    {
        Unit spawned = Instantiate(_unit);
        float x = Random.Range(_leftDownPointSpawn.position.x, _rightUpPointSpawn.position.x);
        float z = Random.Range(_leftDownPointSpawn.position.z, _rightUpPointSpawn.position.z);
        spawned.transform.position = new Vector3(x, 1, z);
        spawned.PickedUp += _base.InsertPickPool;
        spawned.Init(_base);
        _units.Add(spawned);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public void RemoveUnit(Unit unit) 
    { 
        _units.Remove(unit);
    }

    public Unit FreeUnit()
    {
        foreach (Unit unit in _units)
        {
            if (!unit.IsBusy)
            {
                return unit;
            }
        }

        return null;
    }
}
