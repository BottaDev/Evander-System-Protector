using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanPattern : MonoBehaviour
{
    public BossPhase[] bossPhases;


    private Animator animator;
    private BossEntity boss;

    private float accumulatedRotation;
    private float currentRate = 0;
    private float currentPatternDuration;

    private int currentPattern = 0;
    private int currentPhase = 0;
    [SerializeField]
    private int currentAnimation = 0;

    private bool isChangingPattern = false;     // If it's true, the boss will not shoot
    private bool isExecutingAnimation = false;

    [System.Serializable]
    public class BossPhase
    {
        public string name = "Phase";
        public BossPattern[] patterns;
        public int hpToChange;          // HP that must be reached to change phase
    }

    [System.Serializable]
    public class BossPattern
    {
        public string name = "Pattern";
        public float duration;
        public AnimationClip[] animationAttaks;
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
        animator = GetComponent<Animator>();
        boss = GetComponent<BossEntity>();
        float baseHP = boss.baseHP;

        for (int i = 0; i < bossPhases.Length; i++)
        {
            if (bossPhases[i].hpToChange >= baseHP || bossPhases[i].hpToChange < 0)
                Debug.LogError("Error with HP logic in the Boss's pattern. Phase index: " + i);

            for (int j = bossPhases[i].patterns.Length - 1; j >= 0; j--)
            {
                if (bossPhases[i].patterns[j].duration < 0)
                    Debug.LogError("Error with Duration logic in the Boss's pattern. Phase index: " + i + ", Pattern:" + j);
            }
        }

        currentPatternDuration = bossPhases[currentPhase].patterns[currentPattern].duration;

        boss.RegisterPhaseCheckEvent(CheckPhase);
    }


    private void Update()
    {
        if (!isChangingPattern)
        {
            if (bossPhases[currentPhase].patterns[currentPattern].animationAttaks.Length <= 0)
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
            else
            {
                if (!isExecutingAnimation)
                    ExecuteAnimationAttack();
            }
        }

        accumulatedRotation += Time.deltaTime * bossPhases[currentPhase].patterns[currentPattern].rotationPerSecond;
        if (accumulatedRotation >= 360f)
            accumulatedRotation -= 360f;
    }

    private void ExecuteAnimationAttack()
    {
        currentAnimation = UnityEngine.Random.Range(0, bossPhases[currentPhase].patterns[currentPattern].animationAttaks.Length);
        animator.Play(bossPhases[currentPhase].patterns[currentPattern].animationAttaks[currentAnimation].name);

        StartCoroutine(WaitForAnimation());   
    }

    private void SpawnProjectiles()
    {
        float angle = 360f / bossPhases[currentPhase].patterns[currentPattern].numberOfProjectiles;

        for (int i = 0; i < bossPhases[currentPhase].patterns[currentPattern].numberOfProjectiles; i++)
        {
            Quaternion rotation;

            if (!bossPhases[currentPhase].patterns[currentPattern].changeDirection)
                rotation = Quaternion.AngleAxis(i * angle + accumulatedRotation, Vector3.up);
            else
                rotation = Quaternion.AngleAxis(i * angle - accumulatedRotation, Vector3.up);

            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + (direction * bossPhases[currentPhase].patterns[currentPattern].radius);
            Instantiate(bossPhases[currentPhase].patterns[currentPattern].projectile, position, rotation);
        }

        // ToDo for Botta: Check if sounds should be played here instead in BossEntity

        boss.audioSource.PlayOneShot(boss.sounds[0]);               // sounds[0] ---> bullet sound
        currentRate = bossPhases[currentPhase].patterns[currentPattern].fireRate;
    }

    // Checks the current HP of the boss
    public void CheckPhase()
    {
        if (boss.currentHP == 0)
            return;

        if (boss.currentHP <= bossPhases[currentPhase].hpToChange)
        {
            currentPhase++;
            currentPattern = 0;
            currentPatternDuration = bossPhases[currentPhase].patterns[currentPattern].duration;
        }
    }

    private void ChangePattern()
    {
        if (currentPattern == bossPhases[currentPhase].patterns.Length - 1)
            currentPattern = 0;
        else
            currentPattern++;

        currentPatternDuration = bossPhases[currentPhase].patterns[currentPattern].duration;

        if(bossPhases[currentPhase].patterns[currentPattern].waitTime > 0)
            StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        isChangingPattern = true;
        yield return new WaitForSeconds(bossPhases[currentPhase].patterns[currentPattern].waitTime);
        isChangingPattern = false;
    }

    private IEnumerator WaitForAnimation()
    {
        isExecutingAnimation = true;
        print("Esperando");
        yield return new WaitForSeconds(bossPhases[currentPhase].patterns[currentPattern].animationAttaks[currentAnimation].length + bossPhases[currentPhase].patterns[currentPattern].waitTime);
        print("Dejo de esperar");
        isExecutingAnimation = false;
    }
}
