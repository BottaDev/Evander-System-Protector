using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melissa_AttackPattern : AttackPattern
{
    GameObject player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Move()
    {
        if (bossPhases[currentPhase].wayPoints.Length <= 0)
            return;

        agent.speed = boss.movementSpeed;


        if //Follow Player
        (
            bossPhases[currentPhase].patterns[currentPattern] == bossPhases[0].patterns[1] ||
            bossPhases[currentPhase].patterns[currentPattern] == bossPhases[1].patterns[2] ||
            bossPhases[currentPhase].patterns[currentPattern] == bossPhases[2].patterns[3] //||
            //bossPhases[currentPhase].patterns[currentPattern] == bossPhases[2].patterns[5]
        ) 
        {
            agent.destination = player.transform.position;
            return;
        }
        else
        if (Vector3.Distance(bossPhases[currentPhase].wayPoints[currentWayPoint].position, transform.position) < agent.stoppingDistance)
        {
            currentWayPoint++;
            if (currentWayPoint > bossPhases[currentPhase].wayPoints.Length - 1)
                currentWayPoint = 0;
        }

        agent.destination = bossPhases[currentPhase].wayPoints[currentWayPoint].position;
    }
}
