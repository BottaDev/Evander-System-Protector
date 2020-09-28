﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Screen")]
    public GameObject youLoseScreen;
    public GameObject youWinScreen;
    [Header("Bullet Count")]
    public GameObject countBullets;
    public TMPro.TextMeshProUGUI bulletsCountText;

    public void ShowFinalGui(bool win)
    {
        if (win)
            youWinScreen.SetActive(true);
        else
            youLoseScreen.SetActive(true);
    }

    public void ShowAmmo(int ammo)
    {
        if (ammo > 0)
            bulletsCountText.text = ammo.ToString();
        else
            bulletsCountText.text = "NULL";
    }
}
