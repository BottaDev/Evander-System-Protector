using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Type shotType;
    public float speed = 15f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (shotType)
        {
            case Type.PlayerShot:
                if (other.gameObject.layer == 9)
                {
                    // Hacer daño al boss
                }
                break;

            case Type.EnemyShot:
                if (other.gameObject.layer == 8)
                {
                    // Hacer daño al player
                }
                break;
        }

        Destroy(gameObject);
    }

    public enum Type
    {
        PlayerShot,
        EnemyShot,
    }
}
