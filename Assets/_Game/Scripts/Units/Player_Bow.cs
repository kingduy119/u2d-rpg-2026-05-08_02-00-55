using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Bow : MonoBehaviour
{
    // private Animator anim;
    public Transform shootPoint;
    public GameObject arrowPrefab;

    public float shootCooldown = .5f;
    public float shootTimer = 0f;
    public Player player;

    Vector2 _direction;

    // void Awake()
    // {
    //     player = GetComponent<Player>();
    // }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        HandleAiming();

        if (Keyboard.current.kKey.wasPressedThisFrame && shootTimer <= 0)
        {
            player.anim.SetBool("isShooting", true);
            player.isShooting = true;
        }
    }

    // void OnEnable()
    // {
    //     player.anim.SetLayerWeight(0, 0);
    //     player.anim.SetLayerWeight(1, 1);
    // }

    // void OnDisable()
    // {
    //     player.anim.SetLayerWeight(0, 1);
    //     player.anim.SetLayerWeight(1, 0);
    // }

    private void HandleAiming()
    {
        Vector2 direction = InputController.Instance.GetDirection();
        if (direction.x != 0 || direction.y != 0)
        {
            _direction = direction;
            player.anim.SetFloat("aimX", direction.x);
            player.anim.SetFloat("aimY", direction.y);
        }
    }

    public void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity).GetComponent<Arrow>();
        shootTimer = shootCooldown;
        arrow.direction = _direction.normalized;
        player.anim.SetBool("isShooting", false);
    }


}
