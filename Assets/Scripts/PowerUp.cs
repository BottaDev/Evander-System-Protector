using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Stats")]
    public float timeToDestroy = 10f;
    protected float currentSpeed = 5;

    public virtual void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ApplyPowerUp(other.gameObject);
        else if (other.gameObject.layer == 10 || other.gameObject.layer == 15)
            Destroy(gameObject);
    }


    public virtual void ApplyPowerUp(GameObject player) { }
}
