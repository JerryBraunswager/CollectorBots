using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class Bases : MonoBehaviour
{
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private Resorces _resorces;
    [SerializeField] private Base _base;
    [SerializeField] private List<Base> _allBases = new List<Base>();

    public ReadOnlyCollection<Base> AllBases => _allBases.AsReadOnly();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Base home))
            {
                if(!_allBases.Contains(home))
                {
                    _allBases.Add(home);
                }
            }
        }
    }

    public Base AddBase()
    {
        Base spawned = Instantiate(_base, transform);
        spawned.Init(_flagPlacer, _resorces);
        spawned.transform.position = transform.position;
        _allBases.Add(spawned);
        return spawned;
    }
}
