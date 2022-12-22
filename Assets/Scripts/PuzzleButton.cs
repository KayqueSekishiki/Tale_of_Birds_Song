using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    private Animator anim;
    public Animator barrierAnim;

    public LayerMask layer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnPressed()
    {
        anim.SetBool("isPressed", true);
        barrierAnim.SetBool("isPressed", true);

    }

    void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("isPressed", false);

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            OnPressed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            OnExit();
        }
    }

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f, layer);

        if (hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        }
    }

    private void FixedUpdate()
    {
        OnCollision();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
