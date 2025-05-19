using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public CardDetails[] allCards; // Inspector'dan 7 kart� atayaca��z
    public CardUI[] cardSlots; // Paneldeki 3 adet bo� kart objesi

    void OnEnable()
    {
        ShowRandomCards();
    }

    void ShowRandomCards()
    {
        // 3 farkl� rastgele kart se�elim
        CardDetails[] selectedCards = GetRandomUniqueCards(3);

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].Setup(selectedCards[i]);
        }
    }

    CardDetails[] GetRandomUniqueCards(int count)
    {
        CardDetails[] result = new CardDetails[count];
        var tempList = new System.Collections.Generic.List<CardDetails>(allCards);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, tempList.Count);
            result[i] = tempList[index];
            tempList.RemoveAt(index); // Ayn� kart ��kmas�n diye siliyoruz
        }

        return result;
    }
}
