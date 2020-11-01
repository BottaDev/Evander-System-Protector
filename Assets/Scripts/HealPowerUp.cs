using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    public int healAmount = 5;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ApplyPowerUp(other.gameObject);
    }

    private void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerEntity>().Heal(healAmount);

        Destroy(gameObject);
    }
}
