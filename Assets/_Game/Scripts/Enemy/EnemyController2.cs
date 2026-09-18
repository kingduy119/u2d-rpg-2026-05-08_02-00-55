using UnityEngine;
using Characters;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyController2 : MonoBehaviour
{

    public Transform Target { get; set; }
    public Vector2 Direction { get; private set; } = Vector2.zero;

    private Character _character;
    private CircleCollider2D _detectColider;

    public LayerMask _targetLayer;
    public float DetectRange = 3f;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _detectColider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        if (_character != null)
        {
            // _detectColider.radius = _character.ShareData.Combat.AttackRange;
            _detectColider.radius = DetectRange;
        }
    }

    private void Update()
    {
        CheckTarget();
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

    private void CheckTarget()
    {
        if (Target != null)
        {
            Direction = (Target.position - transform.position).normalized;
        }
        else Direction = Vector2.zero;

        _character.Move(Direction);
    }


    public void CheckTargetInAttackRange()
    {
        float detectRange = _character.ShareData.Combat.AttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            _character.AttackPoint.position,
            detectRange,
            _targetLayer);

        if (colliders.Length > 0)
        {
            Transform target = colliders[0].transform;
            float distance = Vector2.Distance(transform.position, target.position);
            // IsPlayerInAttackRange = distance <= detectRange;
            if (distance <= detectRange)
            {
                _character.Attack();
                Debug.Log("Chrat attack");
            }
        }
        else
        {
            Debug.Log("Target not in range");
            // IsPlayerInAttackRange = false;
        }
    }
}
