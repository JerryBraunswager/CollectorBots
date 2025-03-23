using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Base _base;

    private Unit _deliver;
    private bool _isAssigned = false;
    private Base _home;
    private Ground _ground;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            if(unit == _deliver)
            {
                Base spawned = Instantiate(_base, _ground.transform);
                spawned.transform.position = transform.position;
                _ground.AddBase(spawned);
                unit.Init(spawned);
                unit.Deliver();
                //_home.InitFlag(null);
                Destroy(gameObject);
            }
        }
    }

    public void Init(Unit deliver, Base home, Ground ground)
    {
        if (_isAssigned == false)
        {
            _home = home;
            _ground = ground;
            _deliver = deliver;
            _deliver.ProvideTarget(transform);
            _isAssigned = true;
        }
    }
}
