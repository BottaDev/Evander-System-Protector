﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveInput;
    private Camera mainCamera;
    private float currentFireRate = 0;
    private float currentBlinkRate = 0;
    [SerializeField]
    private bool debuggedMovement = false;
    private PlayerEntity player;

    private void Awake()
    {
        player = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0) && currentFireRate <= 0)
            Shoot();
        else
            currentFireRate -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && (moveInput.x != 0 || moveInput.z != 0) && currentBlinkRate <= 0)
            Blink();
        else
            currentBlinkRate -= Time.deltaTime;
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

        if (Physics.Raycast(transform.position, direction, out hit, player.blinkDistance, layerMask))
        {
            Debug.Log("Blink interference");
            Vector3 nextPosition = CalculateBlinkDirection(hit.distance);
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.speed);
        }
        else
        {
            Debug.Log("No blink interference");
            Vector3 nextPosition = CalculateBlinkDirection();
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.speed);
        }

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
            transform.Translate(moveInput * Time.fixedDeltaTime * player.speed, Space.World);
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
