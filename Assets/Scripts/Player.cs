using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rig;
    private PlayerAudio playerAudio;
    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    public float radius;
    public float speed;
    public float jumpForce;

    private Health healthSystem;

    private bool isJumping;
    private bool doubleJumping;
    private bool isAttacking;
    private bool recovery;

    private static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(this);

        }

    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        healthSystem = GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        recoveryCount += Time.deltaTime;
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
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if (doubleJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJumping = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
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
            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {

                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();

                }

                if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();

                }
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


        if (recoveryCount >= 2f)
        {
            anim.SetTrigger("hit");
            healthSystem.health--;
            recoveryCount = 0;
        }



        if (healthSystem.health <= 0 && !recovery)
        {
            recovery = true;
            anim.SetTrigger("death");
            GameController.instance.ShowGameOver();
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
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

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameController.instance.GetCoin();
            Destroy(collision.gameObject, 1f);
        }

        if (collision.CompareTag("Apple"))
        {
            Debug.Log("Peguei a maçã");
            healthSystem.health++;
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            Destroy(collision.gameObject, 1f);
        }

        if (collision.gameObject.layer == 12)
        {
            PlayerPosition.instance.CheckPoint();
            StonePosition.instance.CheckPoint();
        }
    }
}
