using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedBossShot : ShotController
{
    Transform target;
    float starTime;
    bool hasAimed = false;

    private void Start()
    {
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
            speed = 15;
        }else
        {
            speed = Mathf.Exp(-(Time.time - starTime) * 3.5f) * 30;
        }
    }
}
