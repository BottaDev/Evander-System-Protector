﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    private AttackPattern pattern;
    [HideInInspector]
    public Transform player;

    public delegate void PhaseCheckEvent();
    public PhaseCheckEvent onPhaseCheck;
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

        onPhaseCheck?.Invoke();

        bool isSecondPhaseActive = false;
        if (currentHP  <= baseHP/2 && !isSecondPhaseActive)
        {
            isSecondPhaseActive = true;
            onPhaseSwitch?.Invoke();
        }
    }

    virtual public void RegisterPhaseCheckEvent(PhaseCheckEvent newEvent)
    {
        onPhaseCheck += newEvent;
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
