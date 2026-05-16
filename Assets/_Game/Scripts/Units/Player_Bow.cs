using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Bow : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject arrowPrefab;

    public float shootCooldown = .5f;
    public float shootTimer = 0f;
    Vector2 _direction;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        HandleAiming();

        if (InputController.Instance.GetKeyboard().kKey.wasPressedThisFrame && shootTimer <= 0)
        {
            Shoot();
        }
    }

    private void HandleAiming()
    {
        Vector2 direction = InputController.Instance.GetDirection();
        if (direction.x != 0 || direction.y != 0)
        {
            _direction = direction;
        }
    }

    void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity).GetComponent<Arrow>();
        shootTimer = shootCooldown;
        arrow.direction = _direction.normalized;
    }


}
