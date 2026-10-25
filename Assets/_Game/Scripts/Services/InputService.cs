using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputService
{

    public Vector3 MouseWorldPosition => GetMousePosition();
    public bool MouseOver => EventSystem.current.IsPointerOverGameObject() || !Camera.main;
    public bool IsMouseDown => Input.GetMouseButtonDown(0);
    public bool IsMouseUp => Input.GetMouseButtonUp(0);


    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

    public Collider2D PointerCollider2D(string layer) => PointerCollider2D(LayerMask.GetMask(layer));
    public Collider2D PointerCollider2D(LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            MouseWorldPosition,
            Vector2.zero, Mathf.Infinity, layer);
        return hit.collider;
    }
}

