using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : BaseEntity
{
    [Header("Player Stats")]
    public float fireRate = 0.3f;
    public float skillRate = 1f;
    public float blinkDistance = 2;
    public int pellets;

    [Header("Objects")]
    public GameObject shotPrefab;
    public Transform shotSpawn;
    public GameObject barrier;
    public GameObject boss;
    [HideInInspector] public GameObject currentShotPrefab;

    [HideInInspector] public bool canBeDamaged = true;

    [Header("Skill")]
    public Skill currentSkill;
    public Skill nextSkill;

    private bool hasPowerUp = false;
    private float baseFireRate;
    private HealthBar healthBar;
    private UIManager uiManager;
    private float damageStayReset = 2f;
    private float damageStayCounter;
    private ChangeImageSkill skillChange;

    public override void Start()
    {
        base.Start();

        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealt(baseHP);

        uiManager = levelManager.gameObject.GetComponent<UIManager>();

        baseFireRate = fireRate;
        currentShotPrefab = shotPrefab;

        boss.GetComponent<BossEntity>().RegisterPhaseSwitchEvent(onBossPhaseSwitch);
        skillChange = GameObject.Find("GamePlay Canvas").GetComponent<ChangeImageSkill>();
        skillChange.CheckSkill(currentSkill);
    }

    public void SetToBaseFireRate()
    {
        hasPowerUp = false;

        fireRate = baseFireRate;
        currentShotPrefab = shotPrefab;
    }

    public void ChangeGun(float bulletFireRate,  int bullets, GameObject prefab)
    {
        fireRate = bulletFireRate;
        pellets = bullets;
        currentShotPrefab = prefab;

        hasPowerUp = true;

        uiManager.ShowAmmo(bullets);
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
            uiManager.ShowAmmo(0);
            SetToBaseFireRate();
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > baseHP)
            currentHP = baseHP;

        healthBar.SetHealth(currentHP);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 15)
            TakeDamage(1);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Enemy Wall collision
        if (collision.gameObject.layer == 15)
        {
            if (damageStayCounter <= 0)
            {
                damageStayCounter = damageStayReset;
                TakeDamage(1);
            }
            else
                damageStayCounter -= Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Enemy Wall collision
        if (collision.gameObject.layer == 15)
            damageStayCounter = damageStayReset;
    }

    protected override IEnumerator KillEntity()
    {
        Animator animator = meshRenderer.gameObject.GetComponent<Animator>();

        animator.SetBool("isDeath", true);
        yield return new WaitForSeconds(0.7f);

        GameObject particleSystem = Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(particleSystem, 1.5f);

        levelManager.WinLoseGame(gameObject);

        Destroy(gameObject);
    }

    private void onBossPhaseSwitch()
    {
        currentSkill = nextSkill;
        skillChange.CheckSkill(currentSkill);
    }

    public enum Skill
    {
        Blink,
        BlankBullet,
        Teleport,
    }
}
