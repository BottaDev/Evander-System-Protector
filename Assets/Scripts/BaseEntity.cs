using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamagable<float>
{
    public float baseHP;
    [SerializeField]
    protected float currentHP;
    protected float movementSpeed;

    protected Color defaultColor;

    public LevelManager level;

    [SerializeField]
    public AudioClip[] sounds;
    public AudioSource audioSource;

    public virtual void Awake()
    {
        currentHP = baseHP;
        audioSource = GetComponent<AudioSource>();
        defaultColor = GetComponent<MeshRenderer>().material.color;
    }

    virtual public void TakeDamage(float damage)
    {
        currentHP -= damage;
        StartCoroutine(DamageBlink());
        if (currentHP <= 0)
        {
            level.WinLoseGame(gameObject);
            Destroy(gameObject);
        }
    }

    // Change the color to RED when damaged
    virtual protected IEnumerator DamageBlink()
    {
        audioSource.PlayOneShot(sounds[1]); //sounds[1] is the hurt sound

        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;

        yield return null;
    }
}
