using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerUps;

    private float timeToSpawn = 15;
    private float currentTimeToSpawn;
    private GameObject boss;

    public Vector3 center;
    public Vector3 size;

    private void Awake()
    {
        currentTimeToSpawn = timeToSpawn;
    }

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        
        if (boss == null)
            Debug.LogError("Object with tag 'Boss' not finded");
    }

    private void Update()
    {
        currentTimeToSpawn -= Time.deltaTime;

        if(currentTimeToSpawn <= 0)
        {
            Vector3 pos = new Vector3(0, 1, 0);

            int randomPowerUp = Random.Range(0, powerUps.Length);

            float rotationY = Random.Range(0, 361);

            Quaternion rotation = new Quaternion(0, rotationY, 0, 0);

            Instantiate(powerUps[randomPowerUp], pos, rotation);

            currentTimeToSpawn = timeToSpawn;
        }
    }
}