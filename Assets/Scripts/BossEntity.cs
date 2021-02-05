using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntity : BaseEntity
{
    private AttackPattern pattern;
    private Transform player;

    public delegate void PhaseSwitchEvent();
    public PhaseSwitchEvent onPhaseSwitch;

    private GameObject currentModel;
    private HealthBar healthBar;

    private Flamethrower flame;
    private float damageStayCounter;
    private float damageStayReset = 0.3f;

    public  override void Awake()
    {
        base.Awake();

        healthBar = GameObject.Find("Boss HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealt(baseHP);

        pattern = GetComponent<AttackPattern>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 18 && flame == null)
            flame = other.gameObject.GetComponent<Flamethrower>();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 18)
        {
            if (damageStayCounter <= 0)
            {
                damageStayCounter = damageStayReset;
                TakeDamage(flame.damage);
            }
            else
                damageStayCounter -= Time.deltaTime;
        }
    }
}
