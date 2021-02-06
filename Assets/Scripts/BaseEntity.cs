using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamagable<float>
{
    [Header("Entity Stats")]
    public float baseHP;
    public float currentHP;
    public float movementSpeed;

    protected Color defaultColor;
    protected MeshRenderer meshRenderer; 
    
    [Header("Audio Options")]
    public AudioClip[] sounds;
    [HideInInspector]
    public AudioSource audioSource;

    [Header("Death Objects")]
    public GameObject deathParticle;

    protected LevelManager levelManager;

    public virtual void Awake()
    {
        currentHP = baseHP;
        audioSource = GetComponent<AudioSource>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    public virtual void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    virtual public void TakeDamage(float damage)
    {
        if (currentHP <= 0)
            return;

        currentHP -= damage;
        StartCoroutine(DamageBlink());
        if (currentHP <= 0)
            StartCoroutine(KillEntity());
    }

    protected virtual IEnumerator KillEntity()
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

        levelManager.WinLoseGame(gameObject);

        gameObject.SetActive(false);
    }

    // Change the color to RED when damaged
    virtual protected IEnumerator DamageBlink()
    {
        if (sounds.Length > 0)
            audioSource.PlayOneShot(sounds[1]); //sounds[1] is the hurt sound

        meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        meshRenderer.material.color = defaultColor;

        yield return null;
    }
}
