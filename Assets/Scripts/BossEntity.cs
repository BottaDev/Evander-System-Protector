using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    private AttackPattern pattern;

    public  override void Awake()
    {
        base.Awake();

        pattern = GetComponent<AttackPattern>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        pattern.CheckPattern(currentHP);   
    }
}
