using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Ground _ground;
    [SerializeField] private BaseFactory _bases;
    [SerializeField] private Flag _flag;
    [SerializeField] private UnitSpawner _spawner;

    private Base _choosedBase;
    private Unit _deliver;
    private bool _isAssigned = false;

    public Base ChoosedBase => _choosedBase;

    private void OnEnable()
    {
        _ground.MouseClicked += PlaceFlag;
        _spawner.SteppedUp += SpawnBase;

        foreach(Base home in _bases.AllBases) 
        {
            home.BaseChoosed += ChooseBase;
        }
    }

    private void OnDisable()
    {
        _ground.MouseClicked -= PlaceFlag;
        _spawner.SteppedUp -= SpawnBase;

        foreach (Base home in _bases.AllBases)
        {
            home.BaseChoosed -= ChooseBase;
        }
    }

    public bool Init(Unit deliver)
    {
        if (_isAssigned == false && deliver != null)
        {
            _deliver = deliver;
            _deliver.ProvideTarget(transform);
            _isAssigned = true;
            return true;
        }

        return false;
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

        _flag.gameObject.SetActive(true);
        transform.position = new Vector3(position.x, 0, position.z);
    }

    private void SpawnBase(Unit unit)
    {
        if (unit == _deliver)
        {
            Base newBase = _bases.AddBase();
            newBase.transform.position = _flag.transform.position;
            _choosedBase = null;
            newBase.BaseChoosed += ChooseBase;
            unit.Init(newBase.transform);
            unit.Deliver();
            _flag.gameObject.SetActive(false);
            _isAssigned = false;
            _deliver = null;
        }
    }
}
