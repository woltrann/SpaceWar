using UnityEngine;

public class SkillParentController : MonoBehaviour
{
    public SkillDragHandler[] skillButtons; // Inspector'dan atanacak

    void Start()
    {
        CheckUnlockedSkills();
    }

    // T�m skilleri kontrol et, a��lm�� olanlar� a�
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

    // SkillID'ye g�re butonu a�
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
