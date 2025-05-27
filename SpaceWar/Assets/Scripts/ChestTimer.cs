using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Random = UnityEngine.Random;


public class ChestTimer : MonoBehaviour
{
    [Header("Ayarlar")]
    public int chestID ; // Örneðin: "1" = 4 saatlik kasa, "2" = 8 saatlik kasa
    public float hoursToUnlock = 4;

    [Header("UI")]
    public Text timerText;
    public Button openChestButton;

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


        // Buraya kasa ödülünü ver
        if (chestID == 1)
        {
            int randomValue = Random.Range(0, 100); // 0 dahil, 100 hariç
            Debug.Log("rasgele sayý:"+randomValue);

            if (randomValue < 68)
            {
                // %68 ihtimal: 1000 ile 3000 arasýnda gold
                int goldAmount = Random.Range(1000, 3001); // 3000 dahil
                Debug.Log("Gold Kazanýldý: " + goldAmount);
            }
            else if (randomValue < 83) // 68 + 15 = 83
            {
                // %15 ihtimal: 1. 2. 3. skillerden biri
                int skillIndex = Random.Range(1, 4); // 1, 2, 3
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 93) // 83 + 10 = 93
            {
                // %10 ihtimal: 4. 5. 6. skillerden biri
                int skillIndex = Random.Range(4, 7); // 4, 5, 6
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 98) // 93 + 5 = 98
            {
                // %5 ihtimal: 7. 8. 9. skillerden biri
                int skillIndex = Random.Range(7, 10); // 7, 8, 9
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else // %2 ihtimal: 98 - 99
            {
                int skillIndex = Random.Range(10, 13); // 10, 11, 12
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
        }
        else if (chestID == 2)
        {
            int randomValue = Random.Range(0, 100); // 0 dahil, 100 hariç
            Debug.Log("rasgele sayý:" + randomValue);

            if (randomValue < 56)
            {
                // %68 ihtimal: 1000 ile 3000 arasýnda gold
                int goldAmount = Random.Range(1000, 3001); // 3000 dahil
                Debug.Log("Gold Kazanýldý: " + goldAmount);
            }
            else if (randomValue < 74) // 68 + 15 = 83
            {
                // %15 ihtimal: 1. 2. 3. skillerden biri
                int skillIndex = Random.Range(1, 4); // 1, 2, 3
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 87) // 83 + 10 = 93
            {
                // %10 ihtimal: 4. 5. 6. skillerden biri
                int skillIndex = Random.Range(4, 7); // 4, 5, 6
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 95) // 93 + 5 = 98
            {
                // %5 ihtimal: 7. 8. 9. skillerden biri
                int skillIndex = Random.Range(7, 10); // 7, 8, 9
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else // %2 ihtimal: 98 - 99
            {
                int skillIndex = Random.Range(10, 13); // 10, 11, 12
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }

        }
        else
        {
            int randomValue = Random.Range(0, 100); // 0 dahil, 100 hariç
            Debug.Log("rasgele sayý:" + randomValue);

            if (randomValue < 44)
            {
                // %68 ihtimal: 1000 ile 3000 arasýnda gold
                int goldAmount = Random.Range(1000, 3001); // 3000 dahil
                Debug.Log("Gold Kazanýldý: " + goldAmount);
            }
            else if (randomValue < 65) // 68 + 15 = 83
            {
                // %15 ihtimal: 1. 2. 3. skillerden biri
                int skillIndex = Random.Range(1, 4); // 1, 2, 3
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 81) // 83 + 10 = 93
            {
                // %10 ihtimal: 4. 5. 6. skillerden biri
                int skillIndex = Random.Range(4, 7); // 4, 5, 6
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else if (randomValue < 92) // 93 + 5 = 98
            {
                // %5 ihtimal: 7. 8. 9. skillerden biri
                int skillIndex = Random.Range(7, 10); // 7, 8, 9
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }
            else // %2 ihtimal: 98 - 99
            {
                int skillIndex = Random.Range(10, 13); // 10, 11, 12
                Debug.Log("Skill Kazanýldý: Skill " + skillIndex);
            }

        }
        
        
        // Kasa zamanlayýcýsýný sýfýrla
            unlockTime = DateTime.Now.AddHours(hoursToUnlock);
        PlayerPrefs.SetString(chestKey, unlockTime.ToString());
        PlayerPrefs.Save();

        openChestButton.interactable = false;
        chestReady = false;
    }
}
