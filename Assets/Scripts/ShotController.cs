using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Type shotType;
    public float speed = 15f;
    public float damage = 1;

    protected virtual void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        switch (shotType)
        {
            case Type.PlayerShot:
                if (other.gameObject.layer == 9)
                {
                    BaseEntity enemy = other.gameObject.GetComponent<BaseEntity>();
                    enemy.TakeDamage(damage);
                }
                break;

            case Type.EnemyShot:
                if (other.gameObject.layer == 8)
                {
                    PlayerEntity player = other.gameObject.GetComponent<PlayerEntity>();
                    player.TakeDamage(damage);
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
