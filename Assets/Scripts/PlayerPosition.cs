using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private Vector3 checkpoint;

    private void Start()
    {
        checkpoint = transform.position;
    }

    public void Respawn()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            rb.linearVelocity = Vector2.zero;
            rb.position = checkpoint;
        }
    }
}