using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;

    private Boolean _inMainMenu = true;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void toggleMenu(GameObject targetMenu)
    {
        if (_inMainMenu)
        {
            _mainMenu.SetActive(false);
            targetMenu.SetActive(true);
            _inMainMenu = false;
        }
        else
        {
            _mainMenu.SetActive(true);
            targetMenu.SetActive(false);
            _inMainMenu = true;
        }
    }
}
