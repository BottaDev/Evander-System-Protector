using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{

    public Material playerBulletMaterial;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11)
        {
            print("collided with bullet");
            BouncingShot bshot = other.collider.GetComponent<BouncingShot>();
            if (bshot)
            {
                print("collided with bounce bullet");
                bshot.maxBounces += 2;
                bshot.shotType = ShotController.Type.PlayerShot;
                other.collider.GetComponent<MeshRenderer>().material = playerBulletMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            BouncingShot bshot = other.GetComponent<BouncingShot>();
            if (bshot)
            {
                bshot.maxBounces += 2;
                bshot.shotType = ShotController.Type.PlayerShot;
                bshot.damage = bshot.damage * 2;

                other.gameObject.layer = 12;
                other.GetComponent<MeshRenderer>().material = playerBulletMaterial;
            }
        }
    }
}
