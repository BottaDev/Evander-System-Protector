using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : PowerUp
{
    public int healAmount = 5;

    public override void ApplyPowerUp(GameObject player)
    {
        player.GetComponent<PlayerEntity>().Heal(healAmount);

        Destroy(gameObject);
    }
}
