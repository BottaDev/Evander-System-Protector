using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    private AttackPattern pattern;
    private Transform player;

    public delegate void PhaseSwitchEvent();
    public PhaseSwitchEvent onPhaseSwitch;

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

        bool isSecondPhaseActive = false;
        if (currentHP  <= baseHP/2 && !isSecondPhaseActive)
        {
            isSecondPhaseActive = true;
            onPhaseSwitch?.Invoke();
        }
    }

    virtual public void RegisterPhaseSwitchEvent(PhaseSwitchEvent newEvent)
    {
        onPhaseSwitch += newEvent;
    }

    virtual public void ForgetPhaseSwitchEvent(PhaseSwitchEvent eventToRemove)
    {
        onPhaseSwitch -= eventToRemove;
    }

}
