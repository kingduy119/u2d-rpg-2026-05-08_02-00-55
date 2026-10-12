using UnityEngine;
using UnityEngine.InputSystem;

public class Archer_Shoot : MonoBehaviour
{
    [Header("Shooting points")]
    public GameObject arrowPrefab;
    public Transform vertical;
    public Transform horizontal;
    public Transform diagonalUp;
    public Transform diagonalDown;

    public float cooldown = 1f;
    private float cooldownTimer = 0f;
    Vector2 _direction = Vector2.right;
    public Animator anim;
    public GameObject target;


    void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        if (Keyboard.current.kKey.wasPressedThisFrame && cooldownTimer <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (target != null)
            _direction = (target.transform.position - transform.position).normalized;

        anim.SetBool("isShoot", true);
        anim.SetFloat("aimX", _direction.x);
        anim.SetFloat("aimY", _direction.y);

    }
    public void Shoot_Done()
    {
        Transform shootPoint = GetShootPoint();
        if (shootPoint != null)
        {
            Arrow arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.direction = _direction.normalized;
            cooldownTimer = cooldown;
            anim.SetBool("isShoot", false);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction.x != 0 || direction.y != 0)
        {
            _direction = direction;
        }
    }

    Transform GetShootPoint()
    {
        if (_direction.x == 0 && _direction.y > 0 || _direction.x == 0 && _direction.y < 0)
            return vertical;
        else if (_direction.x > 0 && _direction.y == 0 || _direction.x < 0 && _direction.y == 0)
            return horizontal;
        else if (_direction.x > 0 && _direction.y > 0 || _direction.x < 0 && _direction.y > 0)
            return diagonalUp;
        else if (_direction.x > 0 && _direction.y < 0 || _direction.x < 0 && _direction.y < 0)
            return diagonalDown;

        return horizontal; // Default case, should not happen
    }
}
