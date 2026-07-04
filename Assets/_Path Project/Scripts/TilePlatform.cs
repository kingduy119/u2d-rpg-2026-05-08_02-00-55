using UnityEngine;
// using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TDGame
{
    public class TilePlatform : MonoBehaviour
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color unActiveColor;
        [SerializeField] private Collider2D collier;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool canPlacePlatform = true;
        Vector3 prevPosition;

        private bool isDragging;
        private Vector3 offset;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canPlacePlatform = false;
            spriteRenderer.color = unActiveColor;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            canPlacePlatform = true;
            spriteRenderer.color = activeColor;
        }

        private void OnMouseDown()
        {
            isDragging = true;
            prevPosition = transform.position;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = transform.position.z;

            offset = transform.position - mousePos;
        }

        private void OnMouseDrag()
        {
            if (!isDragging) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            transform.position = mousePos + offset;
        }

        private void OnMouseUp()
        {
            if (!canPlacePlatform) transform.position = prevPosition;
            isDragging = false;
        }
    }
}
