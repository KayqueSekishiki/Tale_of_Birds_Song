using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rig;
    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    public float radius;
    public float speed;
    public float jumpForce;
    public int health;

    private bool isJumping;
    private bool doubleJumping;

    private bool isAttacking;
    private bool recovery;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }



    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJumping = true;
            }
            else if (doubleJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJumping = false;
            }

        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("transition", 3);

            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            if (hit != null)
            {
                hit.GetComponent<Slime>().OnHit();
            }

            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.333f);
        isAttacking = false;
    }


    float recoveryCount;
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;

        if (recoveryCount >= 2f)
        {
            anim.SetTrigger("hit");
            health--;
            recoveryCount = 0;
        }

        if (health <= 0 && !recovery)
        {
            speed = 0;
            recovery = true;
            anim.SetTrigger("death");
            //  Destroy(gameObject, 1f);
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            OnHit();
        }
    }
}
