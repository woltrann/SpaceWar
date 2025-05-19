using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CardDetails", menuName = "Stats/CardStats")]
public class CardDetails : ScriptableObject
{
    public string cardName;
    public LocalizedString cardDescription; // Artýk dil destekli
    public Sprite cardImage;
    public int cardAttackSpeed;
    public int cardHealth;
    public int cardShipSpeed;
    public int cardMagnetArea;
    public int cardAttack;
    public int cardAttackRange;
}
