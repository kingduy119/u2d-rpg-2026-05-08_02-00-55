using UnityEngine;
using Characters;


[RequireComponent(typeof(Character))]
// [RequireComponent(typeof(CircleCollider2D))]
public class EnemyController2 : MonoBehaviour
{
    public LayerMask TargetLayer;
    public Transform StartPosition;
    // public float DetectRange = 3f;

    public Transform Target { get; set; }
    public ICter Character { get; private set; }

    public StateMachine States = new();
    public IState ChaseState;
    public IState WanderState;

    private void Awake()
    {
        if (TryGetComponent<Character>(out var character))
        {
            character.SetTargetLayer(TargetLayer);
            Character = character;
        }
        // if (TryGetComponent<CircleCollider2D>(out var colider))
        // {
        //     colider.radius = Character.SO.Combat.DetectionRange;
        // }
    }

    private void Start()
    {
        ChaseState = new EnemyChaseState(this);
        WanderState = new EnemyWanderState(this);
        States.Initialize(ChaseState);
    }

    private void Update()
    {
        CheckTargetInRange();
        States.Execute();
    }

    private void CheckTargetInRange()
    {
        var colider = Physics2D.OverlapCircle(
            transform.position,
            Character.SO.Combat.DetectionRange,
            TargetLayer);

        if (colider) Target = colider.transform;
        else Target = null;
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         Target = collision.transform;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         Target = null;
    //     }
    // }
}

