using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("SCREENS")]
    public GameObject menuScreen;
    public GameObject playerScreen;
    public GameObject youLoseScreen;
    public GameObject youWinScreen;
    public GameObject startScreen;
    [Header("BULLET COUNT")]
    public GameObject countBullets;
    public TMPro.TextMeshProUGUI bulletsCountText;
    [Header("SKILL CHANGE")]
    public GameObject boss;
    public GameObject skillChangeTextPanel;
    [Header("SKILL IMAGES")]
    public Image flashSkill;
    public Image blankSkill;
    public Image tpSkill;
    public Image reflectionSkill;
    public Image timeSlowSkill;

    private float fireRate;
    private Image currentSkillIndicator;
    private bool inCooldown = false;
    private PlayerEntity player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerEntity>();

        SetSkillImage();
    }

    private void Update()
    {
        if (inCooldown)
        {
            currentSkillIndicator.fillAmount += 1 / fireRate * Time.deltaTime;

            if (currentSkillIndicator.fillAmount >= 1)
            {
                inCooldown = false;
                currentSkillIndicator.fillAmount = 1;
            }
        }
    }

    public void ShowFinalGui(bool win)
    {
        if (win)
            youWinScreen.SetActive(true);
        else
            youLoseScreen.SetActive(true);

        menuScreen.SetActive(true);
        playerScreen.SetActive(false);
    }

    public void ShowAmmo(int ammo)
    {
        if (ammo > 0)
        {
            bulletsCountText.text = ammo.ToString();
            Color color;
            if (ColorUtility.TryParseHtmlString("#22AA55", out color))
                bulletsCountText.color = color;
        }
        else
        {
            bulletsCountText.text = "NULL";
            bulletsCountText.color = Color.red;
        }
    }

    // Starts the initial count down
    public void StartCountDown(string bossName)
    {
        playerScreen.SetActive(false);
        startScreen.SetActive(true);

        startScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = bossName;
    }

    public void EndCountDown()
    {
        playerScreen.SetActive(true);
        startScreen.SetActive(false);
    }

    private void Awake()
    {
        boss.GetComponent<BossEntity>().RegisterPhaseSwitchEvent(onBossPhaseSwitch);
    }

    void onBossPhaseSwitch()
    {
        boss.GetComponent<BossEntity>().ForgetPhaseSwitchEvent(onBossPhaseSwitch);
        skillChangeTextPanel.SetActive(true);
    }

    public void SetSkillImage()
    {
        switch (player.currentSkill)
        {
            default:
            case PlayerEntity.Skill.Blink:
                currentSkillIndicator = flashSkill;
                break;

            case PlayerEntity.Skill.BlankBullet:
                currentSkillIndicator = blankSkill;
                break;

            case PlayerEntity.Skill.Teleport:
                currentSkillIndicator = tpSkill;
                break;

            case PlayerEntity.Skill.Reflector:
                currentSkillIndicator = reflectionSkill;
                break;

            case PlayerEntity.Skill.Timestop:
                currentSkillIndicator = timeSlowSkill;
                break;

            case PlayerEntity.Skill.Tranquilizer:

                break;

            case PlayerEntity.Skill.Wall:

                break;

            case PlayerEntity.Skill.Flamethrower:

                break;
        }
    }

    public void StartCd(float amount)
    {
        currentSkillIndicator.fillAmount = 0f;
        fireRate = amount;
        inCooldown = true;
    }
}
