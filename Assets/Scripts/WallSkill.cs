using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill : MonoBehaviour
{
    public float duration;

    private void Start()
    {
        Destroy(gameObject, duration);

        if (transform.position.y > 0 || transform.position.y < 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
