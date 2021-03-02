using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string bossName;
    public float startCountDownDuration = 3f;

    private AttackPattern bossPattern;
    private PlayerInput player;

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerInput>();
        bossPattern= FindObjectOfType<AttackPattern>();

        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        player.enabled = false;
        bossPattern.enabled = false;

        uiManager.StartCountDown(bossName);

        yield return new WaitForSeconds(startCountDownDuration);

        uiManager.EndCountDown();

        player.enabled = true;
        bossPattern.enabled = true;
    }

    public void WinLoseGame(GameObject entity, bool isSasser = false)
    {
        if (entity.layer == 9)
        {
            if (!isSasser && (GameObject.FindGameObjectWithTag("Boss").GetComponent<AttackPattern>().enabled = false) == true)
                Debug.LogWarning("Can't turn off Boss AttackPattern!");

            uiManager.ShowFinalGui(true);
        }
        else if (entity.layer == 8)
        {
            uiManager.ShowFinalGui(false);
        }            
    }
}
