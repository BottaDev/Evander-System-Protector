﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : BaseEntity
{
    [Header("Player Stats")]
    public float fireRate = 0.3f;
    public float blinkRate = 1f;
    public float blinkDistance = 2;
    public float shotSpeed;
    public float shotDamage;
    public int pellets;

    [Header("Objects")]
    public GameObject shotPrefab;
    public Transform shotSpawn;
    public UIManager manager;

    [HideInInspector]
    public bool canBeDamaged = true;

    private bool hasPowerUp = false;
    private float baseFireRate;

    private void Start()
    {
        baseFireRate = fireRate;
    }

    public void SetToBaseFireRate()
    {
        hasPowerUp = false;
        fireRate = baseFireRate;
    }

    public void ChangeGun(float bulletFireRate, float bulletSpeed, float bulletDamage, int bullets)
    {
        fireRate = bulletFireRate;
        shotSpeed = bulletSpeed;
        shotDamage = bulletDamage;
        pellets = bullets;

        hasPowerUp = true;
    }

    public void SetInvulnerability(bool isVulnerable)
    {
        canBeDamaged = isVulnerable;
    }

    void ChangedSkill() 
    {
    }

    public override void TakeDamage(float damage)
    {
        if (canBeDamaged)
            base.TakeDamage(damage);
    }

    public void CheckGunAmmo()
    {
        pellets--;
        manager.ShowAmmo(pellets);

        if (pellets <= 0 && hasPowerUp)
        {
            manager.CheckPowerUpActive(false);
            SetToBaseFireRate();
        }
    }
}
