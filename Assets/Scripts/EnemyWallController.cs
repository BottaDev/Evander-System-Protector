using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallController : MonoBehaviour
{
    public float movementSpeed;
    public Transform[] wayPoints;
    public float stopDistance = 1;

    private int currentWayPoint;

    private void Update()
    {
        if (wayPoints.Length <= 0)
            return;

        if (Vector3.Distance(wayPoints[currentWayPoint].position, transform.position) < stopDistance)
        {
            currentWayPoint++;
            if (currentWayPoint > wayPoints.Length - 1)
                currentWayPoint = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentWayPoint].position, movementSpeed * Time.deltaTime);
    }
}
