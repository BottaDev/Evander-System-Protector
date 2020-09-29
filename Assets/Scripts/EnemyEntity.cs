﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEntity : BaseEntity
{      
    [Header("Enemy Stats")]
    public float fireRate = 0;
    [Header("Objects")]
    public GameObject shotPrefab;
    public Transform shotSpawn;

    private NavMeshAgent agent;
    private float currentFireRate;
    private GameObject player;    

    public override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        agent.speed = movementSpeed;
        currentFireRate = UnityEngine.Random.Range(0f, 1.5f);
    }

    private void Update()
    {
        if (currentFireRate <= 0)
            Shoot();
        else
            currentFireRate -= Time.deltaTime;

        Move();
    }

    private void Move()
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3f * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
            agent.destination = player.transform.position;
    }

    private void Shoot()
    {
        Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation);

        currentFireRate = fireRate;
    }

    public override void TakeDamage(float damage)
    {
        currentHP -= damage;
        StartCoroutine(DamageBlink());
        if (currentHP <= 0)
            Destroy(gameObject);
    }
}