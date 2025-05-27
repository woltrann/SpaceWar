using UnityEngine;
using UnityEngine.UI;

public class SkillPurchaseHandler : MonoBehaviour
{
    public int skillPrice = 100; // Bu skillin fiyatý (Inspector’dan ayarlanýr)
    public SkillDragHandler skillDragHandler; // Ayný objedeki SkillDragHandler referansý
    public Text priceText; // UI'da fiyatý göstermek istersen (opsiyonel)

    void Start()
    {
        // Fiyatý UI'da göstermek istersen:
        if (priceText != null)
            priceText.text = skillPrice + "G";

        // Skill zaten açýlmýþsa, butonu kapat
        if (skillDragHandler != null && skillDragHandler.isUnlocked)
        {
            GetComponent<Button>().interactable = false;
        }

        // Butona týklanma olayýný tanýmlýyoruz
        GetComponent<Button>().onClick.AddListener(TryBuySkill);
    }

    private void TryBuySkill()
    {
        float totalGold = GameManager.Instance.totalGold; // GoldManager üzerinden altýn alýnýr

        if (skillDragHandler == null)
        {
            Debug.LogWarning("SkillDragHandler atanmadý.");
            return;
        }

        if (skillDragHandler.isUnlocked)
        {
            Debug.Log("Bu skill zaten satýn alýnmýþ.");
            return;
        }

        if (totalGold >= skillPrice)
        {
            GameManager.Instance.totalGold -= skillPrice; // Altýn kullanýldý
            GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
            PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
            PlayerPrefs.Save();
            priceText.text = " ";
            skillDragHandler.UnlockSkill();             // Skill'i aktif et
            Debug.Log("Skill satýn alýndý!");
        }
        else
        {
            Debug.Log("Yetersiz altýn.");
        }
    }
}
