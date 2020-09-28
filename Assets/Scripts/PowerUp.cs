using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Type type;
    public float bulletFireRate;
    public float bulletDamage;
    public float bulletSpeed;
    public int bullets;

    public UIManager uiManager;

    private void Start()
    {
        uiManager = GameObject.Find("LevelManager").GetComponent<UIManager>();
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
            ApplyPowerUp(other.gameObject);
    }

    public void ApplyPowerUp(GameObject player)
    {
        switch (type)
        {
            case Type.MachineGun:
                player.GetComponent<PlayerEntity>().ChangeGun(bulletFireRate, bulletSpeed, bulletDamage, bullets);
                uiManager.ShowAmmo(bullets);
                break;
        }

        Destroy(gameObject);
    }

    public enum Type
    {
        MachineGun, 
    }
}
