using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasserAddEntity : BaseEntity
{
    private AttackPattern pattern;
    private Transform player;
    private GameObject currentModel;

    public override void Awake()
    {
        base.Awake();

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

        if (currentHP <= 0)
            return;

        pattern.CheckPattern(currentHP);
    }

    public void ChangeModel(GameObject newModel, int currentPhase)
    {
        currentModel.SetActive(false);

        currentModel = newModel;

        currentModel.SetActive(true);

        meshRenderer = transform.GetChild(currentPhase).GetComponent<MeshRenderer>();
    }

    public void SetBaseHp(int newHp)
    {
        baseHP = newHp;

        currentHP = baseHP;
    }
}
