using UnityEngine;

public class SkillParentController : MonoBehaviour
{
    public SkillDragHandler[] skillButtons; // Inspector'dan atanacak

    void Start()
    {
        CheckUnlockedSkills();
    }

    // Tüm skilleri kontrol et, açýlmýþ olanlarý aç
    public void CheckUnlockedSkills()
    {
        foreach (var skill in skillButtons)
        {
            if (PlayerPrefs.GetInt("SkillUnlocked_" + skill.skillIndex, 0) == 1)
            {
                skill.UnlockSkill();
            }
        }
    }

    // SkillID'ye göre butonu aç
    public void UnlockSkillByID(int skillID)
    {
        foreach (var skill in skillButtons)
        {
            if (skill.skillIndex == skillID)
            {
                skill.UnlockSkill();
                break;
            }
        }
    }
}
