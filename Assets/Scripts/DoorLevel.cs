using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLevel : MonoBehaviour
{
    [SerializeField] private int _levelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health healthSystem = collision.GetComponent<Health>();

            if (healthSystem != null)
            {
                GameController.Instance.SaveProgress(
                    healthSystem.health,
                    healthSystem.heartsCount
                );

                PlayerPrefs.SetInt("level", _levelIndex);
                PlayerPrefs.Save();
            }

            LoadLevel();
        }
    }


    private void LoadLevel()
    {
        SceneManager.LoadScene(_levelIndex);
    }
}