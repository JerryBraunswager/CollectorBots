using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Flag))]
public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Ground _ground;
    [SerializeField] private Bases _bases;

    private Flag _flag;
    private Base _choosedBase;
    private Unit _deliver;
    private bool _isAssigned = false;

    public Base ChoosedBase => _choosedBase;

    private void Awake()
    {
        _flag = GetComponent<Flag>();
    }

    private void OnEnable()
    {
        _ground.MouseClicked += PlaceFlag;
        _flag.SteppedUp += SpawnBase;

        foreach(Base home in _bases.AllBases) 
        {
            home.BaseChoosed += ChooseBase;
        }
    }

    private void OnDisable()
    {
        _ground.MouseClicked -= PlaceFlag;
        _flag.SteppedUp -= SpawnBase;

        foreach (Base home in _bases.AllBases)
        {
            home.BaseChoosed -= ChooseBase;
        }
    }

    public void Init(Unit deliver)
    {
        if (_isAssigned == false)
        {
            _deliver = deliver;
            _deliver.ProvideTarget(transform);
            _isAssigned = true;
        }
    }

    private void ChooseBase(Base home)
    {
        _choosedBase = home;
    }

    private void PlaceFlag(Vector3 position)
    {
        if (_choosedBase == null || _choosedBase.UnitCount <= 1)
        {
            return;
        }

        _flag.ChangeState(true);
        _flag.transform.position = new Vector3(position.x, 0, position.z);
    }

    private void SpawnBase(Unit unit)
    {
        if (unit == _deliver)
        {
            Base newBase = _bases.AddBase();
            newBase.transform.position = _flag.transform.position;
            _choosedBase = null;
            newBase.BaseChoosed += ChooseBase;
            unit.Init(newBase);
            unit.Deliver();
            _flag.ChangeState(false);
            _isAssigned = false;
            _deliver = null;
        }
    }
}
