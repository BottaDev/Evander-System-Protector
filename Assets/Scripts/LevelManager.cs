using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    UIManager manager;

    private int currentLevelIndex;

    private void Awake()
    {
        manager = GetComponent<UIManager>();
    }

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void WinLoseGame(GameObject entity)
    {
        if (entity.layer == 9)
            manager.ShowFinalGui(true);
        else if (entity.layer == 8)
            manager.ShowFinalGui(false);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }
}
