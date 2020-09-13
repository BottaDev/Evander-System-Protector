using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    public BossPattern[] configurations;

    private float accumulatedRotation;
    private int currentPatron = 0;
    private float currentRate = 0;

    private void Start()
    {
        float baseHP = GetComponent<BossEntity>().baseHP;

        for (int i = 0; i < configurations.Length; i++)
        {
            if (configurations[i].hpToChange >= baseHP || configurations[i].hpToChange < 0)
                Debug.LogError("Error with HP logic in the Boss's pattern. Configuration index: " + i);
        }
    }

    [System.Serializable]
    public class BossPattern
    {
        public int hpToChange;          // HP that must be reached to change pattern
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

    private void Update()
    {
        accumulatedRotation += Time.deltaTime * configurations[currentPatron].rotationPerSecond;
        if (accumulatedRotation >= 360f)
            accumulatedRotation -= 360f;

        if (currentRate <= 0)
            SpawnProjectiles();
        else
            currentRate -= Time.deltaTime;
    } 

    private void SpawnProjectiles()
    {
        float angle = 360f / configurations[currentPatron].numberOfProjectiles;

        for (int i = 0; i < configurations[currentPatron].numberOfProjectiles; i++)
        {
            Quaternion rotation;

            if (!configurations[currentPatron].changeDirection)
                rotation = Quaternion.AngleAxis(i * angle + accumulatedRotation, Vector3.up);
            else
                rotation = Quaternion.AngleAxis(i * angle - accumulatedRotation, Vector3.up);

            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + (direction * configurations[currentPatron].radius);
            Instantiate(configurations[currentPatron].projectile, position, rotation);
        }

        currentRate = configurations[currentPatron].fireRate;
    }

    // Checks the current HP of the boss
    public void CheckPattern(float currentHp)
    {
        if (currentHp <= configurations[currentPatron].hpToChange)
        {
            currentPatron++;
        }
    }

}
