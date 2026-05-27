using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private QuestSO questToOffer;
    private bool playerInRange;

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && playerInRange)
        {
            QuestEvents.OnQuestOfferRequested?.Invoke(questToOffer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerẼit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
