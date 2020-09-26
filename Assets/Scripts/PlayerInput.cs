﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool debuggedMovement = false;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Camera mainCamera;
    private ParticleSystem particles;
    private float currentFireRate = 0;
    private float currentBlinkRate = 0;
    private PlayerEntity player;

    private void Awake()
    {
        player = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0) && currentFireRate <= 0)
            Shoot();
        else
            currentFireRate -= Time.deltaTime;
        switch (player.currentSkill)
        {
            case PlayerEntity.Skill.Blink:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && (moveInput.x != 0 || moveInput.z != 0) && currentBlinkRate <= 0)
                    Blink();
                else
                    currentBlinkRate -= Time.deltaTime;
                break;

            case PlayerEntity.Skill.Barrier:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentBlinkRate <= 0)
                    Barrier();
                else
                    currentBlinkRate -= Time.deltaTime;
                break;

            default:
                break;
        }

    }

    private void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void Blink()
    {
        // This would cast rays only against colliders in layer 9
        int layerMask = 1 << 9;
        // But instead we want to collide against everything except layer 9
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.z);

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = transform.position;
        emitParams.applyShapeToPosition = true;
        particles.Emit(emitParams, 50);

        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound

        if (Physics.Raycast(transform.position, direction, out hit, player.blinkDistance, layerMask))
        {
            Debug.Log("Blink interference");
            Vector3 nextPosition = CalculateBlinkDirection(hit.distance);
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.movementSpeed);
        }
        else
        {
            Debug.Log("No blink interference");
            Vector3 nextPosition = CalculateBlinkDirection();
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.movementSpeed);
        }

        currentBlinkRate = player.blinkRate;
        StartCoroutine(SetInvulnerability());
    }

    private void Barrier()
    {
        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound
        GameObject BarrierHB = Instantiate(player.barrier, transform.position, Quaternion.identity);
        BarrierHB.transform.parent = gameObject.transform;

        Destroy(BarrierHB, 0.6f);
        currentBlinkRate = player.blinkRate;
    }

    private void RotatePlayer()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void MovePlayer()
    {
        if (moveInput != Vector3.zero)
        {
            debuggedMovement = false;
            transform.Translate(moveInput * Time.fixedDeltaTime * player.movementSpeed, Space.World);
        }
        else if (moveInput == Vector3.zero && !debuggedMovement)
        {
            debuggedMovement = true;
            rb.velocity = Vector3.zero;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(player.shotPrefab, player.shotSpawn.position, player.shotSpawn.rotation);
        ShotController controller = bullet.GetComponent<ShotController>();
        controller.SetStats(player.shotSpeed, player.shotDamage);

        currentFireRate = player.fireRate;

        player.CheckGunAmmo();

        player.audioSource.PlayOneShot(player.sounds[0]); //player[0]--->bullet sound

        currentFireRate = player.fireRate;
    }

    private IEnumerator SetInvulnerability()
    {
        player.SetInvulnerability(false);
        yield return new WaitForSeconds(0.3f);
        player.SetInvulnerability(true);
    }

    private Vector3 CalculateBlinkDirection(float hitDistance = 1f)
    {
        float auxBlinkDistance = player.blinkDistance;

        if (hitDistance != 1f)
            auxBlinkDistance = hitDistance;

        if (moveInput.x > 0 && moveInput.z > 0)    // Up and Right
        {
            return new Vector3(transform.position.x + auxBlinkDistance, transform.position.y, transform.position.z + auxBlinkDistance);
        }
        else if (moveInput.x > 0 && moveInput.z < 0)    // Down and Right
        {
            return new Vector3(transform.position.x + auxBlinkDistance, transform.position.y, transform.position.z - auxBlinkDistance);
        }
        else if (moveInput.x < 0 && moveInput.z > 0)    // Up and Left
        {
            return new Vector3(transform.position.x - auxBlinkDistance, transform.position.y, transform.position.z + auxBlinkDistance);
        }
        else if (moveInput.x < 0 && moveInput.z < 0)    // Down and Left
        {
            return new Vector3(transform.position.x - auxBlinkDistance, transform.position.y, transform.position.z - auxBlinkDistance);
        }
        else if (moveInput.x > 0)    // Right
        {
            return new Vector3(transform.position.x + auxBlinkDistance, transform.position.y, transform.position.z);
        }
        else if (moveInput.x < 0)   // Left
        {
            return new Vector3(transform.position.x - auxBlinkDistance, transform.position.y, transform.position.z);
        }
        else if (moveInput.z > 0)     // Up
        {
            return new Vector3(transform.position.x, transform.position.y, transform.position.z + auxBlinkDistance);
        }
        else if (moveInput.z < 0)     // Down
        {
            return new Vector3(transform.position.x, transform.position.y, transform.position.z - auxBlinkDistance);
        }

        return Vector3.zero;
    }
}
