using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    protected AttackPattern pattern;
    protected Transform player;

    public delegate void PhaseSwitchEvent();
    public PhaseSwitchEvent onPhaseSwitch;

    protected GameObject currentModel;
    protected HealthBar healthBar;

    public override void Awake()
    {
        base.Awake();

        healthBar = GameObject.Find("Boss HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealt(baseHP);

        pattern = GetComponent<AttackPattern>();
        player = GameObject.Find("Player").GetComponent<Transform>();

        currentModel = gameObject.transform.GetChild(0).gameObject;
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

        if (healthBar != null)
            healthBar.SetHealth(currentHP);

        if (currentHP <= 0)
            return;

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

    public void ChangeModel(GameObject newModel, int currentPhase)
    {
        currentModel.SetActive(false);

        currentModel = newModel;

        currentModel.SetActive(true);

        meshRenderer = transform.GetChild(currentPhase).GetComponent<MeshRenderer>();
    }
}
