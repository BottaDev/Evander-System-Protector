using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageSkill : MonoBehaviour
{
    public GameObject[] listImage;
    public Animator blank;
    public Animator tp;
    public Animator reflector;

    public void CheckSkill(PlayerEntity.Skill skill)
    {
        switch (skill)
        {
            case PlayerEntity.Skill.Blink:
                listImage[0].SetActive(true);
                listImage[1].SetActive(false);
                listImage[2].SetActive(false);
                listImage[3].SetActive(false);
                break;

            case PlayerEntity.Skill.BlankBullet:
                listImage[0].SetActive(false);
                listImage[1].SetActive(true);
                listImage[2].SetActive(false);
                listImage[3].SetActive(false);
                blank.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Teleport:
                listImage[0].SetActive(false);
                listImage[1].SetActive(false);
                listImage[2].SetActive(true);
                listImage[3].SetActive(false);
                tp.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Reflector:
                listImage[0].SetActive(false);
                listImage[1].SetActive(false);
                listImage[2].SetActive(false);
                listImage[3].SetActive(true);
                reflector.SetBool("Change", true);
                break;
        }
    }
}
