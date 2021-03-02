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
    public Animator slow;
    public Animator tranquilizer;
    public Animator wall;
    public Animator flame;

    public void CheckSkill(PlayerEntity.Skill skill)
    {
        switch (skill)
        {
            case PlayerEntity.Skill.Blink:
                listImage[0].SetActive(true);
                break;

            case PlayerEntity.Skill.BlankBullet:
                listImage[0].SetActive(false);
                listImage[1].SetActive(true);
                blank.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Timestop:
                listImage[0].SetActive(false);
                listImage[2].SetActive(true);
                slow.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Teleport:
                listImage[0].SetActive(false);
                listImage[3].SetActive(true);
                tp.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Reflector:
                listImage[0].SetActive(false);
                listImage[4].SetActive(true);
                reflector.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Tranquilizer:
                listImage[0].SetActive(false);
                listImage[5].SetActive(true);
                tranquilizer.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Wall:
                listImage[0].SetActive(false);
                listImage[6].SetActive(true);
                wall.SetBool("Change", true);
                break;

            case PlayerEntity.Skill.Flamethrower:
                listImage[0].SetActive(false);
                listImage[7].SetActive(true);
                flame.SetBool("Change", true);
                break;
        }
    }
}
