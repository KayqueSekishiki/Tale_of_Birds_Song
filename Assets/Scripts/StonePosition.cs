using UnityEngine;

public class StonePosition : MonoBehaviour
{
    public static StonePosition Instance { get; private set; }

    [SerializeField] private Transform stone;

    private Vector3 checkpointPosition;

    private void Awake() => Instance ??= this;

    private void Start()
    {
        stone = GameObject.FindGameObjectWithTag("Stone")?.transform;

        if (stone != null)
            checkpointPosition = stone.position;
    }

    public void SetCheckpoint(Vector3 position) => checkpointPosition = position;

    public void Respawn()
    {
        if (stone == null)
            stone = GameObject.FindGameObjectWithTag("Stone")?.transform;

        if (stone != null)
            stone.position = checkpointPosition;
    }
}