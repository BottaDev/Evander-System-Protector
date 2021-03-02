using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void RetryLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentLevelIndex);
    }

    public void ChangeLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToNextLevel()
    {
        string scneneName = SceneManager.GetActiveScene().name;

        switch (scneneName)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;

            case "Level2":
                SceneManager.LoadScene("Level3");
                break;

            case "Level3":
                SceneManager.LoadScene("Level4");
                break;

            case "Level4":
                SceneManager.LoadScene("Level5");
                break;

            case "Level5":
                SceneManager.LoadScene("Level6");
                break;

            case "Level6":
                SceneManager.LoadScene("Level7");
                break;

            default:
                Debug.LogError("Next level was not found!");
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
