using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public event Action<Vector3> MouseClicked;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        MouseClicked?.Invoke(mousePosition);
    }
}
