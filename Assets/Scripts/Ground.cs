using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public event Action<Vector3> MouseClicked;

    private void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseClicked?.Invoke(mousePosition);
    }
}
