using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("SCREENS")]
    public GameObject menuScreen;
    public GameObject playerScreen;
    public GameObject youLoseScreen;
    public GameObject youWinScreen;
    public GameObject startScreen;
    [Header("BULLET COUNT")]
    public GameObject countBullets;
    public TMPro.TextMeshProUGUI bulletsCountText;
    [Header("SKILL CHANGE")]
    public GameObject boss;
    public GameObject skillChangeTextPanel;

    public void ShowFinalGui(bool win)
    {
        if (win)
            youWinScreen.SetActive(true);
        else
            youLoseScreen.SetActive(true);

        menuScreen.SetActive(true);
        playerScreen.SetActive(false);
    }

    public void ShowAmmo(int ammo)
    {
        if (ammo > 0)
        {
            bulletsCountText.text = ammo.ToString();
            Color color;
            if (ColorUtility.TryParseHtmlString("#22AA55", out color))
                bulletsCountText.color = color;
        }
        else
        {
            bulletsCountText.text = "NULL";
            bulletsCountText.color = Color.red;
        }
    }

    // Starts the initial count down
    public void StartCountDown(string bossName)
    {
        playerScreen.SetActive(false);
        startScreen.SetActive(true);

        startScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = bossName;
    }

    public void EndCountDown()
    {
        playerScreen.SetActive(true);
        startScreen.SetActive(false);
    }

    private void Awake()
    {
        boss.GetComponent<BossEntity>().RegisterPhaseSwitchEvent(onBossPhaseSwitch);
    }

    void onBossPhaseSwitch()
    {
        boss.GetComponent<BossEntity>().ForgetPhaseSwitchEvent(onBossPhaseSwitch);
        skillChangeTextPanel.SetActive(true);
    }
}
