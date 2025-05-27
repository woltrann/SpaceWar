using UnityEngine;
using UnityEngine.UI;

public class SkillPurchaseHandler : MonoBehaviour
{
    public int skillPrice = 100; // Bu skillin fiyat� (Inspector�dan ayarlan�r)
    public SkillDragHandler skillDragHandler; // Ayn� objedeki SkillDragHandler referans�
    public Text priceText; // UI'da fiyat� g�stermek istersen (opsiyonel)

    void Start()
    {
        // Fiyat� UI'da g�stermek istersen:
        if (priceText != null)
            priceText.text = skillPrice + "G";

        // Skill zaten a��lm��sa, butonu kapat
        if (skillDragHandler != null && skillDragHandler.isUnlocked)
        {
            GetComponent<Button>().interactable = false;
        }

        // Butona t�klanma olay�n� tan�ml�yoruz
        GetComponent<Button>().onClick.AddListener(TryBuySkill);
    }

    private void TryBuySkill()
    {
        float totalGold = GameManager.Instance.totalGold; // GoldManager �zerinden alt�n al�n�r

        if (skillDragHandler == null)
        {
            Debug.LogWarning("SkillDragHandler atanmad�.");
            return;
        }

        if (skillDragHandler.isUnlocked)
        {
            Debug.Log("Bu skill zaten sat�n al�nm��.");
            return;
        }

        if (totalGold >= skillPrice)
        {
            GameManager.Instance.totalGold -= skillPrice; // Alt�n kullan�ld�
            GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
            PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);
            PlayerPrefs.Save();
            priceText.text = " ";
            skillDragHandler.UnlockSkill();             // Skill'i aktif et
            Debug.Log("Skill sat�n al�nd�!");
        }
        else
        {
            Debug.Log("Yetersiz alt�n.");
        }
    }
}
