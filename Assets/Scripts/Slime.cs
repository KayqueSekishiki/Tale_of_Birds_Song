using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D _rig;
    private Animator _anim;

    [Header("Detection")]
    [SerializeField] private Transform _point;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _layer;

    [Header("Stats")]
    [SerializeField] private int _health = 1;
    [SerializeField] private float _speed = 2f;

    private bool _movingRight = false;
    private bool _canChangeDirection = true;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckCollision();
    }

    private void Move()
    {
        float direction = _movingRight ? 1 : -1;

        _rig.linearVelocity = new Vector2(
            direction * _speed,
            _rig.linearVelocity.y
        );
    }

    // private void CheckCollision()
    // {
    //     Collider2D hit = Physics2D.OverlapCircle(
    //         _point.position,
    //         _radius,
    //         _layer
    //     );

    //     if (hit != null)
    //         ChangeDirection();
    // }

    private void CheckCollision()
    {
        Vector2 direction = _movingRight
            ? Vector2.right
            : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            _point.position,
            direction,
            0.5f,
            _layer
        );

        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        if (!_canChangeDirection)
            return;

        _canChangeDirection = false;

        _movingRight = !_movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Invoke(nameof(EnableDirectionChange), 0.2f);
    }

    private void EnableDirectionChange()
    {
        _canChangeDirection = true;
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
        _speed = 0;
        _rig.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        _anim.SetTrigger("death");

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        if (_point != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                _point.position,
                _radius
            );
        }
    }
}