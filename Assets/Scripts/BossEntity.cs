using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    private AttackPattern pattern;
    private Transform player;

    public  override void Awake()
    {
        base.Awake();

        pattern = GetComponent<AttackPattern>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            var rotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
        }
            
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        pattern.CheckPattern(currentHP);   
    }
}
