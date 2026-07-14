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
        [SerializeField] private Rigidbody2D _rigidbody;

        private Vector3 m_lastWorldPosition;
        private Vector2 _prevMouse;
        private Vector2 _currentMouse;
        private bool _isDragging;
        private bool _pressed;

        private void Update()
        {
            HandleMouseZoom();

#if UNITY_EDITOR || UNITY_STANDALONE
            // HandleMouse();
            HandleMouseInput();
#else
            HandleTouch();
#endif

            if (_isDragging)
            {
                Vector2 delta = _prevMouse - _currentMouse;
                if (delta.sqrMagnitude > 0.0001f)
                {
                    Move(delta);
                    _prevMouse = _currentMouse;
                }

                // Move(delta);
                // _prevMouse = _currentMouse;
            }
        }

        private void HandleMouseInput()
        {
            if (Mouse.current == null)
                return;

            _currentMouse = GetMouseWorldPosition();

            if (Input.GetMouseButtonDown(0))
            {
                _prevMouse = _currentMouse;
                _pressed = true;
            }


            if (_pressed && _prevMouse != _currentMouse)
            {
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _pressed = false;
                _isDragging = false;
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

        private void HandleMouse()
        {
            // if (Mouse.current == null)
            //     return;

            // // Bắt đầu kéo
            // if (Mouse.current.leftButton.wasPressedThisFrame)
            // {
            //     if (EventSystem.current != null &&
            //         EventSystem.current.IsPointerOverGameObject())
            //         return;

            //     m_lastWorldPosition = GetMouseWorldPosition();
            //     _isDragging = true;
            // }

            // if (_isDragging && Mouse.current.leftButton.isPressed)
            // {
            //     Vector3 currentWorldPosition = GetMouseWorldPosition();
            //     Vector3 delta = m_lastWorldPosition - currentWorldPosition;

            //     transform.position += delta * dragSpeed;
            //     m_lastWorldPosition = currentWorldPosition;
            // }

            // if (Mouse.current.leftButton.wasReleasedThisFrame)
            // {
            //     _isDragging = false;
            // }
        }

        private void HandleTouch()
        {
            if (Touchscreen.current == null)
                return;

            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                m_lastWorldPosition = GetTouchWorldPosition();
                _isDragging = true;
            }

            if (_isDragging && touch.press.isPressed)
            {
                Vector3 currentWorldPosition = GetTouchWorldPosition();

                Vector3 delta = m_lastWorldPosition - currentWorldPosition;

                transform.position += delta * dragSpeed;

                m_lastWorldPosition = currentWorldPosition;
            }

            if (touch.press.wasReleasedThisFrame)
            {
                _isDragging = false;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Vector3 worldPoint = m_renderCamera.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -m_renderCamera.transform.position.z));

            worldPoint.z = transform.position.z;

            return worldPoint;
        }

        private Vector3 GetTouchWorldPosition()
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 world = m_renderCamera.ScreenToWorldPoint(
                new Vector3(touchPos.x, touchPos.y, -m_renderCamera.transform.position.z));

            world.z = transform.position.z;

            return world;
        }

        public void Move(Vector2 delta)
        {
            // _rigidbody.linearVelocity = delta * dragSpeed;
            _rigidbody.MovePosition(_rigidbody.position + delta);
        }
    }
}
