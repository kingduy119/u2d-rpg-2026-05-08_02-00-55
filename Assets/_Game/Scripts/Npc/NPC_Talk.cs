using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interacAnim;
    // public DialogueSO dialogueSO;

    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;

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
                Debug.Log("currentConversation");
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
                conversations.RemoveAt(i);
                currentConversation = con;
            }
        }
    }
}
