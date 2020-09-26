using System;
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
    public GameObject barrier;
    public GameObject boss;

    [HideInInspector]
    public bool canBeDamaged = true;
    public Skill currentSkill;

    private bool hasPowerUp = false;
    private float baseFireRate;
    private HealthBar healthBar;
    private UIManager uiManager;


    public override void Start()
    {
        base.Start();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        uiManager = levelManager.gameObject.GetComponent<UIManager>();
        baseFireRate = fireRate;
        healthBar.SetMaxHealt(baseHP);
        boss.GetComponent<BossEntity>().RegisterPhaseSwitchEvent(onBossPhaseSwitch);
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

    public override void TakeDamage(float damage)
    {
        if (canBeDamaged)
        {
            base.TakeDamage(damage);
            healthBar.SetHealth(currentHP);
        }
    }

    public void CheckGunAmmo()
    {
        pellets--;
        uiManager.ShowAmmo(pellets);

        if (pellets <= 0 && hasPowerUp)
        {
            uiManager.CheckPowerUpActive(false);
            SetToBaseFireRate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
            TakeDamage(1);
    }

    private void onBossPhaseSwitch()
    {
        currentSkill = Skill.Barrier;
    }

    public enum Skill
    {
        Blink,
        Barrier,
    }
}
