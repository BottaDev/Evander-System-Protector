using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillChangeText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string skillName;

    // Start is called before the first frame update
    void OnEnable()
    {
        skillName = GameObject.Find("Player").GetComponent<PlayerEntity>().nextSkill.ToString();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Virus Database Updated - " + skillName + " Skill Adquired";
        StartCoroutine(Slowdown());
    }

    IEnumerator Slowdown()
    {
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSeconds(5 * 0.15f);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
