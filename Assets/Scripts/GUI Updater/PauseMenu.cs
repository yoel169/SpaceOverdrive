using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    public static bool GamePaused = false;

    void Update()
    {
        if (Input.GetButtonDown("Cancel") == true)
        {
            print("cancel detected");

            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        print("pasued");
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        print("resumed");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnResume()
    {
        Resume();
    }
}
