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
    }

    public void SumDeath()
    {
        deathCount++;

        if (deathCount == waves[currentWave].addsToSpawn)
            ChangeWave();
    }

    private void ChangeWave(bool firstTime = false)
    {
        deathCount = 0;

        // Spawn new adds
        for (int i = 0; i < waves[currentWave].addsToSpawn; i++)
        {
            int randomPos = Random.Range(0, spawnPoints.Length);

            GameObject newAdd = Instantiate(add, spawnPoints[randomPos].position, spawnPoints[randomPos].rotation);

            newAdd.GetComponent<SasserAddEntity>().SetBaseHp(addsHp);
        }

        if (!firstTime)
            currentWave++;
    }
}
