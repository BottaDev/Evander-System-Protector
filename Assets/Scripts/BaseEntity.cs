using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamagable<float>
{
    [Header("Life")]
    [SerializeField]
    protected float currentHP;
    [SerializeField]
    protected float baseHP;
    [Header("Speed")]
    protected float movementSpeed;

    protected Color defaultColor;

    [SerializeField]
    public AudioClip[] sounds;
    public AudioSource audioSource;

    virtual protected void Awake()
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
            Destroy(this.gameObject);
    }

    virtual protected IEnumerator DamageBlink()
    {
        audioSource.PlayOneShot(sounds[1]); //sounds[1] is the hurt sound

        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;

        yield return null;
    }
}
