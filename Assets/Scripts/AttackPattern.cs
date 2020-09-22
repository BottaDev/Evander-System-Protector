using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    public BossPhase[] bossConfig;

    private BossEntity boss;
    private float accumulatedRotation;
    private float currentRate = 0;
    private float currentPatternDuration;
    private int currentPattern = 0;
    private int currentPhase = 0;
    private bool isChangingPattron = false;     // If it's true, the boss will not shoot

    [System.Serializable]
    public class BossPhase
    {
        public BossPattern[] patterns;
        public int hpToChange;          // HP that must be reached to change phase
    }

    [System.Serializable]
    public class BossPattern
    {
        public float duration;
        public float waitTime = 1f;         // The time that must pass to execute the following pattern 
        public GameObject projectile;
        [Range(0, 100)]
        public int numberOfProjectiles;
        [Range(0.1f, 1f)]
        public float fireRate = 0.5f;
        [Range(0f, 10f)]
        public float radius;
        [Range(0f, 360f)]
        public float rotationPerSecond = 90;
        public bool changeDirection;
    }


    private void Awake()
    {
        boss = GetComponent<BossEntity>();
        float baseHP = boss.baseHP;

        for (int i = 0; i < bossConfig.Length; i++)
        {
            if (bossConfig[i].hpToChange >= baseHP || bossConfig[i].hpToChange < 0)
                Debug.LogError("Error with HP logic in the Boss's pattern. Phase index: " + i);

            for (int j = bossConfig[i].patterns.Length - 1; j >= 0; j--)
            {
                if (bossConfig[i].patterns[j].duration < 0)
                    Debug.LogError("Error with Duration logic in the Boss's pattern. Phase index: " + i + ", Pattern:" + j);
            }
        }

        currentPatternDuration = bossConfig[currentPhase].patterns[currentPattern].duration;
    }
    
    private void Update()
    {
        if (!isChangingPattron)
        {
            if (currentPatternDuration <= 0)
                ChangePattern();
            else
                currentPatternDuration -= Time.deltaTime;

            if (currentRate <= 0)
                SpawnProjectiles();
            else
                currentRate -= Time.deltaTime;
        }

        accumulatedRotation += Time.deltaTime * bossConfig[currentPhase].patterns[currentPattern].rotationPerSecond;
        if (accumulatedRotation >= 360f)
            accumulatedRotation -= 360f;
    } 

    private void SpawnProjectiles()
    {
        float angle = 360f / bossConfig[currentPhase].patterns[currentPattern].numberOfProjectiles;

        for (int i = 0; i < bossConfig[currentPhase].patterns[currentPattern].numberOfProjectiles; i++)
        {
            Quaternion rotation;

            if (!bossConfig[currentPhase].patterns[currentPattern].changeDirection)
                rotation = Quaternion.AngleAxis(i * angle + accumulatedRotation, Vector3.up);
            else
                rotation = Quaternion.AngleAxis(i * angle - accumulatedRotation, Vector3.up);

            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + (direction * bossConfig[currentPhase].patterns[currentPattern].radius);
            Instantiate(bossConfig[currentPhase].patterns[currentPattern].projectile, position, rotation);
        }

        // ToDo for Botta: Check if sounds should be played here instead in BossEntity

        boss.audioSource.PlayOneShot(boss.sounds[0]);               // sounds[0] ---> bullet sound
        currentRate = bossConfig[currentPhase].patterns[currentPattern].fireRate;
    }

    // Checks the current HP of the boss
    public void CheckPattern(float currentHp)
    {
        if (currentHp == 0)
            return;

        if (currentHp <= bossConfig[currentPhase].hpToChange)
        {
            currentPhase++;
            currentPattern = 0;
            currentPatternDuration = bossConfig[currentPhase].patterns[currentPattern].duration;
        }
    }

    private void ChangePattern()
    {
        if (currentPattern == bossConfig[currentPhase].patterns.Length - 1)
            currentPattern = 0;
        else
            currentPattern++;

        currentPatternDuration = bossConfig[currentPhase].patterns[currentPattern].duration;

        if(bossConfig[currentPhase].patterns[currentPattern].waitTime > 0)
            StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        isChangingPattron = true;
        yield return new WaitForSeconds(bossConfig[currentPhase].patterns[currentPattern].waitTime);
        isChangingPattron = false;
    }
}
