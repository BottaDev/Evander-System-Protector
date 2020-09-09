using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float speed = 15f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)
            Destroy(gameObject);
    }
}
