using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace TDGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera m_renderCamera;
        [SerializeField] private CinemachineCamera m_cinemachineCamera;
        [SerializeField] private float dragSpeed = 1f;
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float minZoom = 3;
        [SerializeField] private float maxZoom = 10;

        private Vector3 m_lastWorldPosition;
        private bool m_isDragging;

        private void Update()
        {
            HandleMouseZoom();

#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        private void HandleMouse()
        {
            if (Mouse.current == null)
                return;

            // Bắt đầu kéo
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject())
                    return;

                m_lastWorldPosition = GetMouseWorldPosition();
                m_isDragging = true;
            }

            // Đang kéo
            if (m_isDragging && Mouse.current.leftButton.isPressed)
            {
                Vector3 currentWorldPosition = GetMouseWorldPosition();

                Vector3 delta = m_lastWorldPosition - currentWorldPosition;

                transform.position += delta * dragSpeed;

                m_lastWorldPosition = currentWorldPosition;
            }

            // Thả chuột
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                m_isDragging = false;
            }
        }

        private void HandleMouseZoom()
        {
            if (Mouse.current == null)
                return;


            float scroll = Mouse.current.scroll.ReadValue().y;

            m_cinemachineCamera.Lens.OrthographicSize -= scroll * zoomSpeed;

            m_cinemachineCamera.Lens.OrthographicSize =
                Mathf.Clamp(m_cinemachineCamera.Lens.OrthographicSize, minZoom, maxZoom);
        }

        private void HandleTouch()
        {
            if (Touchscreen.current == null)
                return;

            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                m_lastWorldPosition = GetTouchWorldPosition();
                m_isDragging = true;
            }

            if (m_isDragging && touch.press.isPressed)
            {
                Vector3 currentWorldPosition = GetTouchWorldPosition();

                Vector3 delta = m_lastWorldPosition - currentWorldPosition;

                transform.position += delta * dragSpeed;

                m_lastWorldPosition = currentWorldPosition;
            }

            if (touch.press.wasReleasedThisFrame)
            {
                m_isDragging = false;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector3 world = m_renderCamera.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -m_renderCamera.transform.position.z));

            world.z = transform.position.z;

            return world;
        }

        private Vector3 GetTouchWorldPosition()
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 world = m_renderCamera.ScreenToWorldPoint(
                new Vector3(touchPos.x, touchPos.y, -m_renderCamera.transform.position.z));

            world.z = transform.position.z;

            return world;
        }
    }
}
