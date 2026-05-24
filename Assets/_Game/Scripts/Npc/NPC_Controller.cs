using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public enum NPCState { Default, Idle, Wander, Patrol, Talk };
    public NPCState currentState = NPCState.Patrol;
    private NPCState previousState;

    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;

    void Start()
    {
        previousState = currentState;
        SwitchState(currentState);
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
