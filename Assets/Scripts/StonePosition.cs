using UnityEngine;

public class StonePosition : MonoBehaviour
{
    private Transform stone;
    private Vector3 checkpointPosition;

    private void Start()
    {
        stone = GameObject.FindGameObjectWithTag("Stone").transform;
        checkpointPosition = stone.position;
    }

    public void Respawn()
    {
        if (stone != null)
        {
            Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = checkpointPosition;
        }
    }
}