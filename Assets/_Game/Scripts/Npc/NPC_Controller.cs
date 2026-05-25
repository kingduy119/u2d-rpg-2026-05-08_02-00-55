using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public enum NPCState { Idle, Wander, Patrol, Talk };
    public NPCState currentState = NPCState.Idle;
    private NPCState previousState;

    private NPC_Patrol patrol;
    private NPC_Wander wander;
    private NPC_Talk talk;

    void Awake()
    {
        patrol = GetComponent<NPC_Patrol>();
        wander = GetComponent<NPC_Wander>();
        talk = GetComponent<NPC_Talk>();
    }

    void Start()
    {
        SwitchState(NPCState.Patrol);
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
            Debug.Log("OnTriggerEnter2D");
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
