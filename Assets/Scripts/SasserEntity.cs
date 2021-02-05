using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasserEntity : BossEntity
{
    [Header("Custom Stats")]
    public Wave[] waves;

    private int currentWave = 0;
    private int deathCount = 0;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] adds; 
    }

    public override void Awake()
    {
        currentHP = baseHP;

        healthBar = GameObject.Find("Boss HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealt(baseHP);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float damage)
    {
        if (currentHP <= 0)
            return;

        currentHP -= damage;

        healthBar.SetHealth(currentHP);

        if (currentHP <= 0)
            StartCoroutine(KillEntity());

        bool isSecondPhaseActive = false;
        if (currentHP <= baseHP / 2 && !isSecondPhaseActive)
        {
            isSecondPhaseActive = true;
            onPhaseSwitch?.Invoke();
        }
    }

    protected override IEnumerator KillEntity()
    {
        levelManager.WinLoseGame(gameObject, true);

        Destroy(gameObject);

        yield return null;
    }

    public void SumDeath()
    {
        deathCount++;

        if (deathCount == waves[currentWave].adds.Length && currentHP > 0)
            ChangeWave();
    }

    private void ChangeWave()
    {
        currentWave++;
        deathCount = 0;

        foreach (GameObject item in waves[currentWave].adds)
        {
            item.SetActive(true);
        }
    }
}
