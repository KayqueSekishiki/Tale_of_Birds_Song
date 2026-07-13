using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _health = 3;
    [SerializeField] private int _heartsCount = 3;

    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _emptyHeart;

    public int health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
            UpdateHearts();
        }
    }

    public int heartsCount
    {
        get => _heartsCount;
        set
        {
            _heartsCount = Mathf.Clamp(value, 0, _maxHealth);
            UpdateHearts();
        }
    }

    private void Start()
    {
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].sprite = i < _health
                ? _fullHeart
                : _emptyHeart;

            _hearts[i].enabled = i < _heartsCount;
        }
    }
}