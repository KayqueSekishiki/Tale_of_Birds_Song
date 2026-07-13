using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField] private Transform player;

    public static PlayerPosition Instance { get; private set; }


    private void Awake() => Instance ??= this;

    private Vector3 _checkpoint;

    private void Start()
    {
        _checkpoint = transform.position;
        Player.Instance.transform.position = _checkpoint;
    }

    public void Respawn()
    {
        Player.Instance.transform.position = _checkpoint;
    }
}