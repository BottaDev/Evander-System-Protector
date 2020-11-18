using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp
{
    [Header("Bullet Stats")]
    public float playerFireRate;
    public int bulletsCount;

    [Header("Bullet Prefab")]
    public GameObject bulletPrefab;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ApplyPowerUp(other.gameObject);
        else if (other.gameObject.layer == 17)
            Destroy(gameObject);
    }

    public override void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerEntity>().ChangeGun(playerFireRate, bulletsCount, bulletPrefab);

        Destroy(gameObject);
    }
}
