using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Unit[] _units;

    private float _score = 0;

    private void Start()
    {
        _text.text = _score.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Resource component))
        {
            _score += component.Score;
            _text.text = _score.ToString();
            component.PickedUp();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Resource resource = Scan();

            if (resource == null) 
            { 
                return; 
            }

            foreach (Unit unit in _units) 
            { 
                if(!unit.IsBusy)
                {
                    unit.ProvideResource(resource);
                    break;
                }
            }
        }
    }

    private Resource Scan()
    {
        Collider[] overlaps = Physics.OverlapBox(Vector3.zero, new Vector3(10, 0, 5), Quaternion.identity, _layerMask.value);
        overlaps = overlaps.OrderBy((collider) => Vector3.Distance(collider.gameObject.transform.position, transform.position)).ToArray();
        Resource returnedObject = null;

        foreach (Collider collider in overlaps)
        {
            if (collider.TryGetComponent(out Resource component))
            {
                if (!component.IsChecked)
                {
                    returnedObject = component;
                    break;
                }
            }
        }
        return returnedObject;
    }
}
