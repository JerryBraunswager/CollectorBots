using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private ResourceFactory _resorces;
    [SerializeField] private Base _base;
    [SerializeField] private UnitSpawner _spawner;
    [SerializeField] private Scaner _scaner;
    [SerializeField] private List<Base> _allBases = new List<Base>();

    public ReadOnlyCollection<Base> AllBases => _allBases.AsReadOnly();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Base home))
            {
                if(_allBases.Contains(home) == false)
                {
                    _allBases.Add(home);
                }
            }
        }
    }

    public Base AddBase()
    {
        Base spawned = Instantiate(_base, transform);
        spawned.Init(_flagPlacer, _resorces, _spawner, _scaner);
        spawned.transform.position = transform.position;
        _allBases.Add(spawned);
        return spawned;
    }
}
