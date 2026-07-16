using UnityEngine;
using System.Collections;

public class Goblin : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _wallLayer;
    private Rigidbody2D _rig;
    private Animator _anim;

    private Vector2 _raycastDirection;

    private bool _isDead;
    private Player currentTarget;

    [SerializeField] private bool _isRight = true;
    [SerializeField] private bool _isFront;

    [SerializeField] private Transform _point;
    [SerializeField] private Transform _behindPoint;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxVision = 5f;
    [SerializeField] private float _stopDistance = 1f;
    [SerializeField] private int _health = 2;
    [SerializeField] private float attackCooldown = 2f;
    private float attackTimer;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
            attackTimer -= Time.fixedDeltaTime;
        DetectPlayer();
        Move();
    }

    private void Move()
    {
        if (_anim.GetInteger("transition") == 3)
            return;

        if (!_isFront || _isDead)
        {
            _rig.linearVelocity = new Vector2(0, _rig.linearVelocity.y);
            return;
        }

        if (CheckWall())
        {
            _rig.linearVelocity = Vector2.zero;
            return;
        }

        _anim.SetInteger("transition", 1);

        float direction = _isRight ? 1 : -1;

        _rig.linearVelocity = new Vector2(
            direction * _speed,
            _rig.linearVelocity.y
        );
    }

    private void DetectPlayer()
    {
        _isFront = false;

        RaycastHit2D frontHit = Physics2D.Raycast(
            _point.position,
            _raycastDirection,
            _maxVision,
            _playerLayer
        );

        if (frontHit.collider != null &&
            frontHit.collider.CompareTag("Player") &&
            !_isDead)
        {
            _isFront = true;

            float distance = Vector2.Distance(
                transform.position,
                frontHit.transform.position
            );

            if (distance <= _stopDistance)
            {
                Attack(frontHit.transform);
            }
            else
            {
                _anim.SetInteger("transition", 1);
            }
        }


        RaycastHit2D behindHit = Physics2D.Raycast(
            _behindPoint.position,
            -_raycastDirection,
            _maxVision,
            _playerLayer
        );

        if (behindHit.collider != null &&
            behindHit.collider.CompareTag("Player"))
        {
            _isRight = !_isRight;
            UpdateDirection();
            _isFront = true;
        }
    }

    private void Attack(Transform player)
    {
        Debug.Log($"Attack chamado | Timer: {attackTimer}");

        if (attackTimer > 0)
            return;

        attackTimer = attackCooldown;

        currentTarget = player.GetComponent<Player>();

        _rig.linearVelocity = Vector2.zero;

        _anim.SetInteger("transition", 3);
    }

    public void DealDamage()
    {
        Debug.Log("Dano chamado");

        if (currentTarget != null)
            currentTarget.OnHit();
    }

    public void EndAttack()
    {
        Debug.Log("Fim do ataque chamado");

        _anim.SetInteger("transition", 0);
    }

    private void UpdateDirection()
    {
        transform.eulerAngles = _isRight
            ? Vector3.zero
            : new Vector3(0, 180, 0);

        _raycastDirection = _isRight
            ? Vector2.right
            : Vector2.left;
    }

    public void OnHit()
    {
        _anim.SetTrigger("hit");

        _health--;

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;

        _speed = 0;

        _rig.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        _anim.SetTrigger("death");

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        if (_point != null)
            Gizmos.DrawRay(_point.position, _raycastDirection * _maxVision);

        if (_behindPoint != null)
            Gizmos.DrawRay(_behindPoint.position, -_raycastDirection * _maxVision);
    }

    private bool CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _point.position,
            _raycastDirection,
            0.5f,
            _wallLayer
        );

        return hit.collider != null;
    }
}