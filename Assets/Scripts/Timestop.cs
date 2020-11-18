using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestop : MonoBehaviour
{

    public bool activeTimePassed;
    public List<GameObject> frozenObjects;

    private void Start()
    {
        frozenObjects = new List<GameObject>();
        Destroy(this.gameObject, 2.6f);
        Destroy(this.GetComponent<CapsuleCollider>(), 0.6f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            other.GetComponent<ShotController>().speed = 0;
            frozenObjects.Add(other.gameObject);
        }
        
        if(other.gameObject.layer == 15)
        {
            other.GetComponent<EnemyWallController>().movementSpeed = 0;
            frozenObjects.Add(other.gameObject);
        }

        StartCoroutine(TimeControl());
        
    }

    private void Update()
    {
        if (activeTimePassed)
        {

            foreach (GameObject go in frozenObjects)
            {
                if (go && go.layer == 11)
                {
                    go.GetComponent<ShotController>().speed = 15;
                }

                if (go && go.gameObject.layer == 15)
                {
                    go.GetComponent<EnemyWallController>().movementSpeed = 12;
                }
            }
        }
    }

    IEnumerator TimeControl()
    {
        yield return new WaitForSeconds(2.5f);
        activeTimePassed = true;

        yield return null;
    }

}
