using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool debuggedMovement = false;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Camera mainCamera;
    private ParticleSystem particles;
    private float currentFireRate = 0;
    private float currentSkillRate = 0;
    private PlayerEntity player;
    private UIManager uiManager;
    private GameObject tpPointInstance;
    private bool tpPointSet = false;

    private bool soundActive = true;

    private void Awake()
    {
        player = GetComponent<PlayerEntity>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        particles = GetComponent<ParticleSystem>();
        uiManager = GameObject.Find("LevelManager").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (player.currentHP <= 0)
            return;

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0) && currentFireRate <= 0)
            Shoot();
        else
            currentFireRate -= Time.deltaTime;

        CheckCurrentSkill();
    }

    private void FixedUpdate()
    {
        if (player.currentHP <= 0)
            return;

        RotatePlayer();
        MovePlayer();
    }

    private void CheckCurrentSkill()
    {
        switch (player.currentSkill)
        {
            default:
            case PlayerEntity.Skill.Blink:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && (moveInput.x != 0 || moveInput.z != 0) && currentSkillRate <= 0)
                    UseBlink();
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.BlankBullet:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                    UseBlankBullet();
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.Teleport:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    if (tpPointSet)
                        UseTeleport();
                    else
                        SetTeleportPoint();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.Reflector:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    UseReflector();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.Timestop:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    UseTimestop();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.Tranquilizer:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    UseTranquilizer();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;
                
            case PlayerEntity.Skill.Wall:
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    UseWall();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }
                break;

            case PlayerEntity.Skill.Flamethrower:
                if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)) && currentSkillRate <= 0)
                {
                    UseFlamethrower();
                }
                else if ((Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift)))
                {
                    CancelFlamethrower();
                }
                else
                {
                    CheckReadySkillSound();
                    currentSkillRate -= Time.deltaTime;
                }                    
                break;
        }
    }

    private void UseBlink()
    {
        // This would cast rays only against colliders in layer 9 and 11
        int aux9 = 1 << 9;
        int aux11 = 1 << 11;
        int layerMask = aux9 | aux11;

        // But instead we want to collide against everything except layer 9 and 11
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.z);

        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound

        if (Physics.Raycast(transform.position, direction, out hit, player.blinkDistance, layerMask))
        {
            Debug.Log("Blink interference, layer: " + hit.collider.gameObject.layer);
            Vector3 nextPosition = CalculateBlinkDirection(hit.distance);
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.movementSpeed);
        }
        else
        {
            Debug.Log("No blink interference");
            Vector3 nextPosition = CalculateBlinkDirection();
            transform.position = Vector3.Lerp(transform.position, nextPosition, player.movementSpeed);
        }

        currentSkillRate = player.blinkRate;
        StartCoroutine(SetInvulnerability());

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = transform.position;
        emitParams.applyShapeToPosition = true;
        particles.Emit(emitParams, 50);

        uiManager.StartCd(player.blinkRate);

        soundActive = false;
    }

    private void UseBlankBullet()
    {
        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound
        GameObject BarrierHB = Instantiate(player.barrier, transform.position, Quaternion.identity);
        BarrierHB.transform.parent = gameObject.transform;

        Destroy(BarrierHB, 0.6f);
        currentSkillRate = player.blankBulletRate;

        uiManager.StartCd(player.blankBulletRate);

        soundActive = false;
    }

    private void UseTimestop()
    {
        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound
        GameObject TimestopHB = Instantiate(player.timestop, transform.position, Quaternion.identity);

        currentSkillRate = player.timeStopRate;

        uiManager.StartCd(player.timeStopRate);

        soundActive = false;
    }

    private void UseReflector()
    {
        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound
        GameObject ReflectorHB = Instantiate(player.reflector, transform.position, Quaternion.identity);
        ReflectorHB.transform.parent = gameObject.transform;
        ReflectorHB.transform.rotation = transform.rotation;

        Destroy(ReflectorHB, 0.5f);
        currentSkillRate = player.reflectorRate;

        uiManager.StartCd(player.reflectorRate);

        soundActive = false;
    }

    private void UseTranquilizer()
    {
        Instantiate(player.tranquilizer, player.shotSpawn.position, player.shotSpawn.rotation);
        player.audioSource.PlayOneShot(player.sounds[0]); //player[0]--->bullet sound
        currentSkillRate = player.tranquilizerRate;

        uiManager.StartCd(player.tranquilizerRate);

        soundActive = false;
    }

    private void UseWall()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            SpawnWall(hitInfo.point);

        player.audioSource.PlayOneShot(player.sounds[0]); //player[0]--->bullet sound
        currentSkillRate = player.wallRate;

        uiManager.StartCd(player.wallRate);

        soundActive = false;
    }

    private void SpawnWall(Vector3 spawnPosition)
    {
        Instantiate(player.wall, spawnPosition, transform.rotation);
    }

    private void UseTeleport()
    {
        player.audioSource.PlayOneShot(player.sounds[2]); //Player.sounds[2] is the blink sound

        Vector3 newPos = new Vector3(tpPointInstance.transform.position.x, transform.position.y, tpPointInstance.transform.position.z);
        transform.position = newPos;

        Destroy(tpPointInstance);

        tpPointSet = false;

        currentSkillRate = player.teleportRate;

        uiManager.StartCd(player.teleportRate);

        soundActive = false;
    }

    private void SetTeleportPoint()
    {
        tpPointSet = true;
        tpPointInstance = Instantiate(player.tpPoint, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
    }

    private void UseFlamethrower()
    {
        player.flametrhower.SetActive(true);
        player.movementSpeed = 6;
    }

    private void CancelFlamethrower()
    {
        player.flametrhower.SetActive(false);
        player.movementSpeed = 12;
        currentSkillRate = player.flamethrowerRate;

        uiManager.StartCd(player.flamethrowerRate);

        soundActive = false;
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
        Instantiate(player.currentShotPrefab, player.shotSpawn.position, player.shotSpawn.rotation);

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

    private void CheckReadySkillSound()
    {
        if (!soundActive && currentSkillRate <= 0)
        {
            soundActive = true;
            player.audioSource.clip = player.sounds[3];
            player.audioSource.Play();
        }
    }
}
