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
            Vector3 pos = CalculateSpawnPosition();

            int randomPowerUp = Random.Range(0, powerUps.Length);

            switch (randomPowerUp)
            {
                case 0:
                    Instantiate(powerUps[0] ,pos , Quaternion.identity);
                    break;

                case 1:
                    Instantiate(powerUps[1], pos, Quaternion.identity);
                    break;
            }

            currentTimeToSpawn = timeToSpawn;
        }
    }

    public Vector3 CalculateSpawnPosition()
    {
        Vector3 pos = Vector3.zero;

        bool canSpawn = false;

        while (!canSpawn)
        {
            pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

            GameObject tempObj = new GameObject("temporary cube with collider (SpawnLocationValid)");
            tempObj.transform.position = pos;
            var bCol = tempObj.AddComponent<SphereCollider>();
            bCol.isTrigger = true;
            bCol.radius = 0.2f;

            Collider[] colls = Physics.OverlapSphere(pos, bCol.radius, 10);

            if (colls.Length == 0)
            {
                GameObject.Destroy(tempObj);
                canSpawn = true;
            }
            else
            {
                canSpawn = false;
                GameObject.Destroy(tempObj);
            }
        }
        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(center, size);
    }
}