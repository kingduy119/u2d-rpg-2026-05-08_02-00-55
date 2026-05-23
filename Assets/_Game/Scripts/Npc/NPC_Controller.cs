using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public enum NPCState { Default, Idle, Wander, Patrol, Talk };
    public NPCState currentState = NPCState.Patrol;
    private NPCState previousState;

    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;

    void Awake()
    {
        patrol = GetComponent<NPC_Patrol>();
        wander = GetComponent<NPC_Wander>();
        talk = GetComponent<NPC_Talk>();
    }

    void Start()
    {
        previousState = currentState;
        SwitchState(currentState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchState(NPCState newState)
    {
        if (currentState == newState) return;

        previousState = currentState;
        currentState = newState;

        patrol.enabled = currentState == NPCState.Patrol;
        wander.enabled = currentState == NPCState.Wander;
        talk.enabled = currentState == NPCState.Talk;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwitchState(NPCState.Talk);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwitchState(previousState);
        }
    }
}
