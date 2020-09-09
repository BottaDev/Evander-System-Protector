using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController1 : MonoBehaviour
{
    public float speed = 15f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9)
            Destroy(gameObject);
    }
}
