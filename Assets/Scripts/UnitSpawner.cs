using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private BaseFactory _allBases;
    [SerializeField] private Unit _unit;

    private List<Unit> _units = new List<Unit>();

    public int UnitCount => _units.Count;
    public event Action<Unit> SteppedUp;

    private void OnDisable()
    {
        foreach (var unit in _units)
        {
            unit.ReachedFlag -= TrySpawnBase;
        }
    }

    public Unit SpawnUnit(Base home, Transform leftDownPointSpawn, Transform rightUpPointSpawn)
    {
        Unit spawned = Instantiate(_unit);
        float x = UnityEngine.Random.Range(leftDownPointSpawn.position.x, rightUpPointSpawn.position.x);
        float z = UnityEngine.Random.Range(leftDownPointSpawn.position.z, rightUpPointSpawn.position.z);
        spawned.transform.position = new Vector3(x, 1, z);
        spawned.ReachedFlag += TrySpawnBase;
        spawned.Init(home.transform);
        _units.Add(spawned);
        return spawned;
    }

    public Unit FreeUnit(Transform home)
    {
        foreach (Unit unit in _units)
        {
            if (home == unit.Home && unit.IsBusy == false)
            {
                return unit;
            }
        }

        return null;
    }

    private void TrySpawnBase(Unit unit)
    {
        SteppedUp?.Invoke(unit);
    }
}
