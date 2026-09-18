using UnityEngine;
using Characters;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyController2 : MonoBehaviour
{

    public Transform Target { get; set; }
    public Vector2 Direction { get; private set; } = Vector2.zero;

    private Character _character;
    private CircleCollider2D _attackRange;

    public LayerMask _targetLayer;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _attackRange = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        if (_character != null)
        {
            _attackRange.radius = _character.ShareData.Combat.AttackRange;
        }
    }

    private void Update()
    {
        CheckTargetInRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Target = null;
            Debug.Log("OnTriggerExit2D - Player");
        }
    }

    private void CheckTargetInRange()
    {
        if (Target != null)
        {
            Direction = (Target.position - transform.position).normalized;
        }
        else Direction = Vector2.zero;

        _character.Move(Direction);


        //     if (Direction.x > 0 && transform.localScale.x < 0 ||
        //    Direction.x < 0 && transform.localScale.x > 0)
        //     {
        //         Flip();
        //     }
    }


    public void CheckPlayerInAttackRange()
    {
        float detectRange = _character.ShareData.Combat.AttackRange;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            _character.AttackPoint.position,
            detectRange,
            _targetLayer);

        if (colliders.Length > 0)
        {
            Target = colliders[0].transform;
            float distance = Vector2.Distance(transform.position, Target.position);
            // IsPlayerInAttackRange = distance <= detectRange;
        }
        else
        {
            // IsPlayerInAttackRange = false;
        }
    }
}
