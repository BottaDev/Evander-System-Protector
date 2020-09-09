using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossPatron[] configurations;
    public float patronRate = 10f;
    
    //awoduijawoduiwhadiouawhuiowahduiowahiud
    private int currentPatron = 0;
    private float currentRate = 0;
    private float currentPatronRate;

    private void Start()
    {
        currentPatronRate = patronRate;
    }

    [System.Serializable]
    public class BossPatron
    {
        public GameObject projectile;
        [Range(0, 100)]
        public int numberOfProjectiles;
        [Range(0.1f, 1f)]
        public float fireRate = 0.5f;
        [Range(0f, 10f)]
        public float radius;
    }

    private void Update()
    {
        if (currentPatronRate <= 0)
            ChangePatron();
        else
            currentPatronRate -= Time.deltaTime;

        if (currentRate <= 0)
            SpawnProjectiles();
        else
            currentRate -= Time.deltaTime;
    }

    private void ChangePatron()
    {
        if (currentPatron == configurations.Length - 1)
            currentPatron = 0;
        else
            currentPatron++;

        currentPatronRate = patronRate;
    }

    private void SpawnProjectiles()
    {
        float angle = 360f / configurations[currentPatron].numberOfProjectiles;

        for (int i = 0; i < configurations[currentPatron].numberOfProjectiles; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + (direction * configurations[currentPatron].radius);
            Instantiate(configurations[currentPatron].projectile, position, rotation);
        }

        currentRate = configurations[currentPatron].fireRate;
    }
}
