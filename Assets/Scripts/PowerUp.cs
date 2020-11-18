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
        float rotationY = Random.Range(0, 361);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);
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
        else if (other.gameObject.layer == 17)
            Destroy(gameObject);
    }

    public virtual void ApplyPowerUp(GameObject player) { }
}
