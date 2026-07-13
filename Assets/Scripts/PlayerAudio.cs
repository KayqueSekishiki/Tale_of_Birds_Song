using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _hitSound;

    public AudioClip coinSound => _coinSound;
    public AudioClip jumpSound => _jumpSound;
    public AudioClip hitSound => _hitSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfx != null)
            _audioSource.PlayOneShot(sfx);
    }
}