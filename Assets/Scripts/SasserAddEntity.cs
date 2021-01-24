using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasserAddEntity : BossEntity
{
    private SasserEntity sasser;

    public override void Awake()
    {
        // Base Entity awake
        audioSource = GetComponent<AudioSource>();
        meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        
        pattern = GetComponent<AttackPattern>();
        player = GameObject.Find("Player").GetComponent<Transform>();

        currentHP = baseHP;

        currentModel = gameObject.transform.GetChild(0).gameObject;
    }

    public override void Start()
    {
        base.Start();

        sasser = GameObject.Find("SASSER").GetComponent<SasserEntity>();
    }

    public override void TakeDamage(float damage)
    {
        if (currentHP <= 0)
            return;

        currentHP -= damage;
        sasser.TakeDamage(damage);

        StartCoroutine(DamageBlink());
        if (currentHP <= 0)
        {
            StartCoroutine(KillEntity());
            return;
        }   

        pattern.CheckPattern(currentHP);
    }

    protected override IEnumerator KillEntity()
    {
        Animator animator = meshRenderer.gameObject.GetComponent<Animator>();

        animator.SetBool("isDeath", true);
        yield return new WaitForSeconds(1f);    // We give the animator time to change the animation

        int animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(animationDuration);

        if (deathParticle != null)
        {
            GameObject particleSystem = Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(particleSystem, 1.5f);
        }

        sasser.SumDeath();

        Destroy(gameObject);
    }

    // Do nothing
    public override void RegisterPhaseSwitchEvent(PhaseSwitchEvent newEvent){ }

    // Do nothing
    public override void ForgetPhaseSwitchEvent(PhaseSwitchEvent eventToRemove) { }
}
