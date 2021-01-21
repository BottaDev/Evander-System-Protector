using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalShotLine : VerticalShot
{
    Vector3 _startPosition = new Vector3(-21, 1, 20);
    public GameObject bullet;

    public override void Awake()
    {
        base.Awake();
        transform.position = _startPosition;
        SetLine();
    }

    void SetLine()
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject go = Instantiate(bullet, _startPosition + Vector3.right * i * 2, Quaternion.identity);
        }

    }
}
