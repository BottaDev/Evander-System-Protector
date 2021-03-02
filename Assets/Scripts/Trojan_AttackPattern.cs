using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trojan_AttackPattern : AttackPattern
{
    bool test = true;
    float baseSpeed;

    protected override void Awake()
    {
        base.Awake();
        baseSpeed = boss.movementSpeed;
    }

    protected override void Move()
    {
        base.Move();

        if (currentWayPoint%2 == 0 && test)
        {
            StartCoroutine("ChessMove");
            test = false;

        }else if(currentWayPoint % 2 != 0 && !test)
        {
            test = true;
        }
    }
    
    protected override void SpawnProjectiles()
    {
        base.SpawnProjectiles();

    }

    IEnumerator ChessMove()
    {
        //print("chess " + currentWayPoint + " " + currentWayPoint%2);
        ChangePattern();
        SpawnProjectiles();

        boss.movementSpeed = 0.0f;
        yield return new WaitForSeconds(0.5f);
        boss.movementSpeed = baseSpeed;
    }
}
