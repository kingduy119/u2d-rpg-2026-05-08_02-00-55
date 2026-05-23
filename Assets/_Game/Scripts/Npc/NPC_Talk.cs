using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interacAnim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        anim.Play("idle");
        interacAnim.Play("speech_open");
    }

    void OnDisable()
    {
        interacAnim.Play("speech_close");
    }
}
