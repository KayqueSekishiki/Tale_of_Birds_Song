using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{

    private Rigidbody2D rig;
    private Animator anim;
    private Vector2 raycastDirection;

    private bool isDead;
    public bool isFront;
    public bool isRight;

    public Transform point;
    public Transform behindPoint;

    public float speed;
    public float maxVision;
    public float stopDistance;
    public int health;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            raycastDirection = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            raycastDirection = Vector2.left;
        }
    }

    private void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }

    void OnMove()
    {
        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);
            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                raycastDirection = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                raycastDirection = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }
    }


    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, raycastDirection, maxVision);

        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player"))
            {

                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance)
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;


                    anim.SetInteger("transition", 3);
                    hit.transform.GetComponent<Player>().OnHit();
                }
            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behindPoint.position, -raycastDirection, maxVision);

        if (behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                isFront = true;
            }
        }

    }


    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0)
        {
            speed = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(point.position, raycastDirection * maxVision);
        Gizmos.DrawRay(behindPoint.position, -raycastDirection * maxVision);
    }
}
