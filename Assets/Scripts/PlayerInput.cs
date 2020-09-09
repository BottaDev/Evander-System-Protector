using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float speed;
    public float fireRate = 0.3f;
    public float blinkRate = 1f;
    public float blinkDistance = 2;

    public GameObject shotPrefab;
    public Transform shotSpawn;


    private Vector3 moveInput;
    private Camera mainCamera;
    private float currentFireRate = 0;
    private float currentBlinkRate = 0;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

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

    private void Blink()
    {
        // This would cast rays only against colliders in layer 9
        int layerMask = 1 << 9;
        // But instead we want to collide against everything except layer 9
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.z);

        if (Physics.Raycast(transform.position, direction, out hit, blinkDistance, layerMask))
        {
            Debug.Log("Blink interference");
            Vector3 nextPosition = CalculateBlinkDirection(hit.distance);
            transform.position = Vector3.Lerp(transform.position, nextPosition, speed);
        }
        else
        {
            Debug.Log("No blink interference");
            Vector3 nextPosition = CalculateBlinkDirection();
            transform.position = Vector3.Lerp(transform.position, nextPosition, speed);
        }

        currentBlinkRate = blinkRate;
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

    private void FixedUpdate()
    {
        RotatePlayer();

        transform.Translate(moveInput * Time.fixedDeltaTime * speed, Space.World);
        //transform.position += moveInput * speed * Time.fixedDeltaTime;
    }

    private void Shoot()
    {
        Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation);

        currentFireRate = fireRate;
    }

    private Vector3 CalculateBlinkDirection(float hitDistance = 1f)
    {
        float auxBlinkDistance = blinkDistance;

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
