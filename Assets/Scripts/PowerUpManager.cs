using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerUps;

    private float timeToSpawn = 15;
    private float currentTimeToSpawn;

    public Vector3 center;
    public Vector3 size;

    private void Awake()
    {
        currentTimeToSpawn = timeToSpawn;
    }

    private void Update()
    {
        currentTimeToSpawn -= Time.deltaTime;

        if(currentTimeToSpawn <= 0)
        {
            Vector3 pos = new Vector3(0, 1, 0);

            int randomPowerUp = Random.Range(0, powerUps.Length);
            
            GameObject go = Instantiate(powerUps[randomPowerUp], pos, Quaternion.identity);

            currentTimeToSpawn = timeToSpawn;
        }
    }
}