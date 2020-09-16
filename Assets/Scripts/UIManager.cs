using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject youLoseScreen;
    public GameObject youWinScreen;
    public GameObject imgBullets;
    public GameObject countBullets;
    public TMPro.TextMeshProUGUI bulletsCountText;

    public PlayerEntity player;

    public void ShowFinalGui(bool win)
    {
        if (win)
            youWinScreen.SetActive(true);
        else
            youLoseScreen.SetActive(false);
    }

    public void CheckPowerUpActive(bool activePowerUp)
    {
        imgBullets.SetActive(activePowerUp);
        countBullets.SetActive(activePowerUp);
    }

    public void ShowAmmo(int ammo)
    {
        if (ammo > 0)
            bulletsCountText.text = ammo.ToString();
    }
}
