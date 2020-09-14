using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public float speed;
    public float fireRate = 0.3f;
    public float blinkRate = 1f;
    public float blinkDistance = 2;
    public float shotSpeed;
    public float shotDamage;
    public int pellets;
    private bool hasPowerUp = false;
    private float baseFireRate;

    public GameObject shotPrefab;
    public Transform shotSpawn;

    private void Start()
    {
        baseFireRate = fireRate;
    }

    private void OnTriggerEnter(Collider collision)
    {
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

    void ChangedSkill() 
    {
    }

    public void CheckGunAmmo()
    {
        pellets--;

        if (pellets <= 0 && hasPowerUp)
            SetToBaseFireRate();
    }
}
