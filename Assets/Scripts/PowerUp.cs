using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Stats")]
    public float timeToDestroy = 10f;

    public virtual void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ApplyPowerUp(other.gameObject);
    }

    public virtual void ApplyPowerUp(GameObject player) { }
}
