using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameisPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameisPaused)
                Resume();
            else
                Pause();
        }
    }

     public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameisPaused = false;
    }

    public void Pause()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameisPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }
}
