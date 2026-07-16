using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.15f; private Rigidbody2D rig;
    private PlayerAudio playerAudio;
    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    public float radius;
    public float speed;
    public float jumpForce;

    [SerializeField] private float attackCooldown = 0.5f;
    private float attackTimer;

    private Health healthSystem;

    private bool isJumping;
    private bool doubleJumping;
    private bool isAttacking;
    private bool recovery;

    private PlayerPosition playerPosition;
    private StonePosition stonePosition;


    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        healthSystem = GetComponent<Health>();

        playerPosition = FindObjectOfType<PlayerPosition>();
        stonePosition = FindObjectOfType<StonePosition>();

        GameController.Instance.RegisterUI(gameOverPanel, scoreText);
    }

    void Update()
    {
        recoveryCount += Time.deltaTime;
        attackTimer -= Time.deltaTime;
        Jump();
        Attack();
        isJumping = !IsGrounded();
    }

    void FixedUpdate()
    {
        Move();
    }



    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement != 0)
        {
            if (!isJumping && !isAttacking)
                anim.SetInteger("transition", 1);

            transform.eulerAngles = movement > 0
                ? Vector3.zero
                : new Vector3(0, 180, 0);
        }
        else if (!isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        bool grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);

        if (grounded)
        {
            isJumping = false;
            doubleJumping = true;
        }
        else
        {
            isJumping = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);

                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                isJumping = true;

                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if (doubleJumping)
            {
                anim.SetInteger("transition", 2);

                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                doubleJumping = false;

                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
        }
    }

    void Attack()
    {
        if (attackTimer > 0)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            attackTimer = attackCooldown;

            isAttacking = true;
            anim.SetInteger("transition", 3);

            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {
                if (hit.TryGetComponent(out Slime slime))
                    slime.OnHit();

                if (hit.TryGetComponent(out Goblin goblin))
                    goblin.OnHit();
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
            playerPosition?.Respawn();
            stonePosition?.Respawn();
            GameController.Instance?.ShowGameOver();
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger com: {collision.name} | Layer: {collision.gameObject.layer}");

        if (collision.gameObject.layer == 9)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            Collider2D col = collision.GetComponent<Collider2D>();

            if (!col.enabled)
                return;

            col.enabled = false;
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameController.Instance?.GetCoin();
            Destroy(collision.gameObject, 1f);
        }

        if (collision.CompareTag("Apple"))
        {
            Collider2D col = collision.GetComponent<Collider2D>();

            if (!col.enabled)
                return;

            col.enabled = false;

            healthSystem.health++;
            healthSystem.heartsCount++;

            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");

            Destroy(collision.gameObject, 1f);
        }

        if (collision.gameObject.layer == 12)
        {
            playerPosition?.Respawn();
            stonePosition?.Respawn();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            0.15f,
            groundLayer);
    }
}
