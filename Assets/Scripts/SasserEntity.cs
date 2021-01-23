using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasserEntity : BossEntity
{
    [Header("Custom Stats")]
    public Wave[] waves;
    public GameObject add;
    public int addsHp;
    public Transform[] spawnPoints;

    private int currentWave = 0;
    private int deathCount = 0;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int addsToSpawn;
    }

    public override void Awake()
    {
        int totalAdds = 0;

        for (int i = 0; i < waves.Length; i++)
        {
            totalAdds +=  waves[i].addsToSpawn;
        }

        baseHP = addsHp * totalAdds;

        currentHP = baseHP;

        healthBar = GameObject.Find("Boss HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealt(baseHP);
    }

    public override void Start()
    {
        base.Start();

        ChangeWave(true);
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

        if (deathCount == waves[currentWave].addsToSpawn && currentHP > 0)
            ChangeWave();
    }

    private void ChangeWave(bool firstTime = false)
    {
        deathCount = 0;

        if (!firstTime)
            currentWave++;

        bool[] posUsed = new bool[spawnPoints.Length];

        // Spawn new adds
        for (int i = 0; i < waves[currentWave].addsToSpawn; i++)
        {
            bool spawned = false;

            do
            {
                int randomPos = Random.Range(0, spawnPoints.Length);

                if (!posUsed[randomPos])
                {
                    GameObject newAdd = Instantiate(add, spawnPoints[randomPos].position, spawnPoints[randomPos].rotation);

                    newAdd.GetComponent<SasserAddEntity>().SetBaseHp(addsHp);

                    posUsed[randomPos] = true;

                    spawned = true;
                }

            } while (!spawned);

        }
    }
}
