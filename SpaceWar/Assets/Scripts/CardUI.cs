using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image cardImage;
    public Text cardNameText;
    public Text cardDescriptionText;

    private CardDetails cardData;


    public void Setup(CardDetails card)
    {
        if (cardData != null)
        {
            // Önceki karta ait event'ten çýk
            cardData.cardDescription.StringChanged -= UpdateCardDescription;
        }
        cardData = card;
        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardName;
        card.cardDescription.StringChanged += UpdateCardDescription;
        card.cardDescription.RefreshString();
    }
    private void UpdateCardDescription(string localizedText)
    {
        cardDescriptionText.text = localizedText;
    }

    public void OnCardSelected()
    {
        // Oyuncuyu bul ve karta göre özellikleri uygula
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerSmoothFollow playerScript = player.GetComponent<PlayerSmoothFollow>();
            playerScript.ApplyCard(cardData);
        }

        // Kart panelini kapatabilirsin istersen
        //transform.parent.gameObject.SetActive(false);
    }
}
