using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interacAnim;
    public DialogueSO dialogueSO;

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
        rb.isKinematic = true;
    }

    void OnDisable()
    {
        interacAnim.Play("speech_close");
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (DialogueManager.Instance.isDialogueActive)
                DialogueManager.Instance.AdvanceDialogue();
            else
                DialogueManager.Instance.StartDialogue(dialogueSO);
        }
    }
}
