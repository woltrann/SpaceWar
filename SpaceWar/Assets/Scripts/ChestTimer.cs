using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Random = UnityEngine.Random;


public class ChestTimer : MonoBehaviour
{
    private SkillDragHandler currentSkillDragHandler;
    public SkillParentController skillParentController;

    [Header("Ayarlar")]
    public int chestID ; // Örneðin: "1" = 4 saatlik kasa, "2" = 8 saatlik kasa
    public float hoursToUnlock = 4;

    [Header("UI")]
    public Sprite[] skillSprites; // skillSprites[0] = Skill1, skillSprites[1] = Skill2, ...
    public String[] skillNames = { "FlashLeap", "Regenerative Core", "Deflector Shield", "Stealth Cloak ", "AutoTurretDeploy", "EMPWave", "MeteorStrike", "MirrorIllusion", "Timelock", "OverdriveMode", "BlackHoleGranade", "NeutronBomb" };
    public Text timerText;
    public Button openChestButton;
    public GameObject ChestResultPanel;
 
    public GameObject ChestImage;
    public Image skillImageUI;
    public Text Reward;

    private string chestKey;
    private DateTime unlockTime;
    private bool chestReady = false;

    void Start()
    {
        chestKey = "ChestUnlockTime" + chestID;

        if (PlayerPrefs.HasKey(chestKey))
        {
            unlockTime = DateTime.Parse(PlayerPrefs.GetString(chestKey));
        }
        else
        {
            unlockTime = DateTime.Now.AddHours(hoursToUnlock);
            PlayerPrefs.SetString(chestKey, unlockTime.ToString());
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (chestReady) return;

        TimeSpan remaining = unlockTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            chestReady = true;
            timerText.text = "Kasa Açýlabilir!";
            openChestButton.interactable = true;
        }
        else
        {
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                remaining.Hours, remaining.Minutes, remaining.Seconds);
        }
    }

    public void OnOpenChest()
    {
        if (!chestReady) return;

        Debug.Log($"KASA {chestID} AÇILDI!");

        string rewardMessage = ""; // Kazanýlan ödül metni

        // Ödül verme
        int randomValue = Random.Range(0, 100);

        if (chestID == 1)
        {
            if (randomValue < 68)
            {
                int goldAmount = Random.Range(1000, 3001);
                GameManager.Instance.totalGold += goldAmount;
                GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
                PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
                rewardMessage = $"+ {goldAmount} G";
            }
            else if (randomValue < 83)
            {
                int skillIndex = Random.Range(1, 4);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 93)
            {
                int skillIndex = Random.Range(4, 7);
                rewardMessage = skillNames[skillIndex-1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 98)
            {
                int skillIndex = Random.Range(7, 10);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else
            {
                int skillIndex = Random.Range(10, 13);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }

        }
        else if (chestID == 2)
        {
            if (randomValue < 56)
            {
                int goldAmount = Random.Range(3000, 5001);
                GameManager.Instance.totalGold += goldAmount;
                GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
                PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
                rewardMessage = $"+ {goldAmount} G";
            }
            else if (randomValue < 74)
            {
                int skillIndex = Random.Range(1, 4);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 87)
            {
                int skillIndex = Random.Range(4, 7);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 95)
            {
                int skillIndex = Random.Range(7, 10);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else
            {
                int skillIndex = Random.Range(10, 13);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
        }
        else
        {
            if (randomValue < 44)
            {
                int goldAmount = Random.Range(5000, 10001);
                GameManager.Instance.totalGold += goldAmount;
                GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
                PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
                rewardMessage = $"+ {goldAmount} G";
            }
            else if (randomValue < 65)
            {
                int skillIndex = Random.Range(1, 4);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 81)
            {
                int skillIndex = Random.Range(4, 7);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else if (randomValue < 92)
            {
                int skillIndex = Random.Range(7, 10);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
            else
            {
                int skillIndex = Random.Range(10, 13);
                rewardMessage = skillNames[skillIndex - 1];
                TrySkills(skillIndex, rewardMessage);
            }
        }

        // Göster panelini
        Reward.text = rewardMessage;
        ChestResultPanel.SetActive(true);
        ChestImage.SetActive(true);
        Invoke(nameof(HideResultPanel), 2f); // 2 saniye sonra paneli kapat

        // Kasa zamanlayýcýsýný sýfýrla
        unlockTime = DateTime.Now.AddHours(hoursToUnlock);
        PlayerPrefs.SetString(chestKey, unlockTime.ToString());
        PlayerPrefs.Save();

        openChestButton.interactable = false;
        chestReady = false;
    }
    void HideResultPanel()
    {
        ChestResultPanel.SetActive(false);
        ChestImage.SetActive(false);
        skillImageUI.gameObject.SetActive(false);

    }
    public void TrySkills(int skillIndex, string rewardMessage)
    {
        // Zaten açýlmýþ mý kontrol et
        if (PlayerPrefs.GetInt("SkillUnlocked_" + skillIndex, 0) == 1)
        {
            // Skill zaten var, altýn ver
            int goldAmount = Random.Range(2000, 8001);
            GameManager.Instance.totalGold += goldAmount;
            GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
            PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
            Reward.text = $"+ {goldAmount} G";

            skillImageUI.gameObject.SetActive(false); // skill zaten varsa görseli kapat

        }
        else
        {
            // Yeni skill kazanýldý, kilidi aç
            PlayerPrefs.SetInt("SkillUnlocked_" + skillIndex, 1);
            PlayerPrefs.Save();
            PlayerPrefs.GetInt("SkillUnlocked_" + skillIndex, 0);

            Reward.text = rewardMessage;
            Debug.Log("Skill açýlýyor: SkillUnlocked_" + skillIndex + " = " + PlayerPrefs.GetInt("SkillUnlocked_" + skillIndex));
            skillParentController.UnlockSkillByID(skillIndex);

            if (skillIndex - 1 < skillSprites.Length && skillIndex > 0)
            {
                skillImageUI.sprite = skillSprites[skillIndex - 1];
                skillImageUI.gameObject.SetActive(true);
            }
        }

    }
}
