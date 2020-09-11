﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamagable<float>
{
    [Header("Life")]
    [SerializeField]
    protected float currentHP;
    [SerializeField]
    protected float baseHP;
    [Header("Speed")]
    protected float movementSpeed; 

    virtual protected void Awake()
    {
        currentHP = baseHP;
    }

    virtual public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Destroy(this.gameObject);
    }
}
