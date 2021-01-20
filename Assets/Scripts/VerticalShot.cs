using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalShot : ShotController
{
    virtual public void Awake()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

}
