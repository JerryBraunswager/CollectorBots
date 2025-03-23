using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private List<Base> _allBases = new List<Base>();
    private Base _choosedBase;

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++) 
        { 
            if(transform.GetChild(i).TryGetComponent(out Base home))
            {
                _allBases.Add(home);
                home.BaseChoosed += ChooseBase;
            }
        }
    }

    private void OnDisable()
    {
        foreach(Base home in _allBases) 
        {
            home.BaseChoosed -= ChooseBase;
        }
    }

    private void OnMouseDown()
    {
        if (_choosedBase == null)
        {
            return;
        }

        Flag flag = null;

        if(_choosedBase.CheckFlag())
        {
            flag = _choosedBase.Flag;
        }
        else
        {
            flag = Instantiate(_flagPrefab);
            _choosedBase.InitFlag(flag);
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        flag.transform.position = new Vector3(mousePosition.x, 0, mousePosition.z);
    }

    public void AddBase(Base newBase)
    {
        _allBases.Add((newBase));
        newBase.BaseChoosed += ChooseBase;
    }

    private void ChooseBase(Base home)
    {
        _choosedBase = home;
    }
}
