using UnityEngine;
using Characters;


[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyController2 : MonoBehaviour
{

    public Transform Target { get; set; }
    private Character _character;
    // public Character Character => _character;

    public ICter Character { get; private set; }

    public LayerMask TargetLayer;
    public Transform StartPosition;
    public float DetectRange = 3f;


    public StateMachine States = new();
    public IState ChaseState;
    public IState WanderState;

    private void Awake()
    {
        if (TryGetComponent<Character>(out var character))
        {
            // Character2 = character;
            character.SetTargetLayer(TargetLayer);
            Character = character;

            // _character = character;
            // _character.SetTargetLayer(TargetLayer);
        }
        if (TryGetComponent<CircleCollider2D>(out var colider))
        {
            colider.radius = DetectRange;
        }
    }

    private void Start()
    {
        ChaseState = new EnemyChaseState(this);
        WanderState = new EnemyWanderState(this);
        States.Initialize(ChaseState);
    }

    private void Update()
    {
        States.Execute();
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
        }
    }
}

