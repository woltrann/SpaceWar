using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SkillDetails", menuName = "Stats/SkillStats")]
public class SkillDetails : ScriptableObject
{
    public string cardName;
    public LocalizedString cardDescription; // Artýk dil destekli
    public Sprite cardImage;

}
