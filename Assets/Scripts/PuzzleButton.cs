using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private Animator _barrierAnim;

    private bool _isPressed;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") && !_isPressed)
        {
            _isPressed = true;
            SetButtonState(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") && _isPressed)
        {
            _isPressed = false;
            SetButtonState(false);
        }
    }

    private void SetButtonState(bool state)
    {
        _anim.SetBool("isPressed", state);
        _barrierAnim.SetBool("isPressed", state);
    }
}