using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamagable<float>
{
    public float baseHP;
    [SerializeField]
    protected float currentHP;
    protected float movementSpeed; 

    public virtual void Awake()
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
