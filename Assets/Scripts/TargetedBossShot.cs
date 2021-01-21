using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedBossShot : ShotController
{
    private Transform target;
    private float starTime;
    private bool hasAimed = false;
    private float baseSpeed; 

    private void Start()
    {
        baseSpeed = speed;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        starTime = Time.time;
    }

    protected override void Update()
    {

        base.Update();

        UpdateSpeed();


        if (speed <= 0.5f)
        {
            transform.LookAt(target);
            hasAimed = true;
        }
    }

    void UpdateSpeed()
    {
        if (hasAimed)
        {
            speed = baseSpeed;
        }else
        {
            speed = Mathf.Exp(-(Time.time - starTime) * 3.5f) * 30;
        }
    }
}
