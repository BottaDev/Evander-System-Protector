using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranquilizerBullet : ShotController
{
    public float slowFactor;
    public float slowDuration;

    protected override void OnTriggerEnter(Collider other)
    {
        switch (shotType)
        {
            case Type.PlayerShot:
                if (other.gameObject.layer == 9)
                {
                    BaseEntity enemy = other.gameObject.GetComponent<BaseEntity>();
                    enemy.TakeDamage(damage);
                    StartCoroutine(Slow(enemy));
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

        if (other.gameObject.layer != 16 && other.gameObject.layer != 9)
        {
            Destroy(gameObject);
            //print("normal death");
        }

    }

    IEnumerator Slow(BaseEntity enemy)
    {
        //print("slow");
        enemy.movementSpeed -= slowFactor;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(slowDuration);
        //print("unslow");
        enemy.movementSpeed += slowFactor;
        Destroy(gameObject);
    }
}
