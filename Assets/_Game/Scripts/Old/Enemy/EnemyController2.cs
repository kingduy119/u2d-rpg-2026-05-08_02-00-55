using UnityEngine;
using Characters;


[RequireComponent(typeof(Character))]
public class EnemyController2 : MonoBehaviour
{
    public LayerMask TargetLayer;
    public Transform StartPosition;

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
    }

    private void Start()
    {
        ChaseState = new EnemyChaseState1(this);
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
}

