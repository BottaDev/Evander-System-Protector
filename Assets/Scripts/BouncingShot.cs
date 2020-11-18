using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BouncingShot : ShotController
{
    public LayerMask bouncingMask;
    public int maxBounces = 1;

    private int bouncingCount = 0;

    protected override void Update()
    {
        base.Update();

        Debug.DrawRay(transform.position, transform.forward, Color.green);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Works like OnTriggerEnter but more precise
        if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.4f, bouncingMask))
        {
            if (bouncingCount == maxBounces)
                Destroy(gameObject);

            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);

            float newRotation = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, newRotation, 0);

            bouncingCount++;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        switch (shotType)
        {
            case Type.PlayerShot:
                if (other.gameObject.layer == 9)
                {
                    BaseEntity enemy = other.gameObject.GetComponent<BaseEntity>();
                    enemy.TakeDamage(damage);

                    Destroy(gameObject);
                }
                break;

            case Type.EnemyShot:
                if (other.gameObject.layer == 8)
                {
                    PlayerEntity player = other.gameObject.GetComponent<PlayerEntity>();
                    player.TakeDamage(damage);

                    Destroy(gameObject);
                }
                break;
        }
    }
}
