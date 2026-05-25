using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interacAnim;
    // public DialogueSO dialogueSO;

    public DialogueSO currentConversation;
    public List<DialogueSO> conversations;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.Play("idle");
        interacAnim.Play("speech_open");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnDisable()
    {
        interacAnim.Play("speech_close");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (DialogueManager.Instance.isDialogueActive)
                DialogueManager.Instance.AdvanceDialogue();
            else
            {
                CheckForNewConversation();
                DialogueManager.Instance.StartDialogue(currentConversation);
            }
        }
    }

    private void CheckForNewConversation()
    {
        for (int i = 0; i < conversations.Count; i++)
        {
            var con = conversations[i];
            if (con != null && con.IsConditionMet())
            {
                currentConversation = con;
                if (con.removeAfterPlay)
                    conversations.RemoveAt(i);

                if (con.removeTheseOnPlay != null && con.removeTheseOnPlay.Count > 0)
                {
                    foreach (var toRemove in con.removeTheseOnPlay)
                    {
                        conversations.Remove(toRemove);
                    }
                }
                currentConversation = con;
                break;
            }
        }
    }
}
