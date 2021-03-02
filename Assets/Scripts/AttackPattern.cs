using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPattern : MonoBehaviour
{
    public BossPhase[] bossPhases;

    protected BossEntity boss;
    private float accumulatedRotation;
    private float currentRate = 0;
    private float currentPatternDuration;
    protected int currentPattern = 0;
    protected int currentPhase = 0;
    protected int currentWayPoint = 0;
    protected NavMeshAgent agent;
    private bool isChangingPattron = false;     // If it's true, the boss will not shoot

    [System.Serializable]
    public class BossPhase
    {
        public string name;
        public BossPattern[] patterns;
        public int hpToChange;          // HP that must be reached to change phase
        public GameObject nextPhaseModel;
        public Transform[] wayPoints;
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

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        boss = GetComponent<BossEntity>() != null ? GetComponent<BossEntity>() : GetComponent<SasserAddEntity>();
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
    }
    
    private void Update()
    {
        if (boss.currentHP <= 0)
            return;

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

        Move();

        accumulatedRotation += Time.deltaTime * bossPhases[currentPhase].patterns[currentPattern].rotationPerSecond;
        if (accumulatedRotation >= 360f)
            accumulatedRotation -= 360f;
    }

    protected virtual void Move()
    {
        if (bossPhases[currentPhase].wayPoints.Length <= 0)
            return;

        agent.speed = boss.movementSpeed;

        if (Vector3.Distance(bossPhases[currentPhase].wayPoints[currentWayPoint].position, transform.position) < agent.stoppingDistance)
        {
            currentWayPoint++;
            if (currentWayPoint > bossPhases[currentPhase].wayPoints.Length - 1)
                currentWayPoint = 0;
        }

        agent.destination = bossPhases[currentPhase].wayPoints[currentWayPoint].position;
    }

    protected virtual void SpawnProjectiles()
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
    public void CheckPattern(float currentHp)
    {
        if (currentHp <= bossPhases[currentPhase].hpToChange)
        {
            if (bossPhases[currentPhase].nextPhaseModel != null)
                boss.ChangeModel(bossPhases[currentPhase].nextPhaseModel, currentPhase + 1);

            currentPhase++;
            currentPattern = 0;
            currentWayPoint = 0;
            currentPatternDuration = bossPhases[currentPhase].patterns[currentPattern].duration;
        }
    }

    protected void ChangePattern()
    {
        if (currentPattern == bossPhases[currentPhase].patterns.Length - 1)
            currentPattern = 0;
        else
            currentPattern++;

        currentPatternDuration = bossPhases[currentPhase].patterns[currentPattern].duration;

        currentRate = bossPhases[currentPhase].patterns[currentPattern].fireRate; 
        //Creeeo que hay un problema en el que el firerate de un patrón hincha las bolas al siguiente cuando se cambia de uno a otro
        //esto debería solucionarlo????

        if (bossPhases[currentPhase].patterns[currentPattern].waitTime > 0)
            StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        isChangingPattron = true;
        yield return new WaitForSeconds(bossPhases[currentPhase].patterns[currentPattern].waitTime);
        isChangingPattron = false;
    }
}
