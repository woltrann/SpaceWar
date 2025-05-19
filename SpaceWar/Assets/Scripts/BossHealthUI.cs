using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Enemy bossEnemy;         // Inspector�dan atayacaks�n
    public Slider healthSlider;     // Inspector�dan atayacaks�n

    void Start()
    {
        if (bossEnemy != null && healthSlider != null)
        {
            healthSlider.maxValue = bossEnemy.maxHealth;
            healthSlider.value = bossEnemy.GetCurrentHealth();
        }
    }

    void Update()
    {
        if (bossEnemy != null && healthSlider != null)
        {
            healthSlider.value = bossEnemy.GetCurrentHealth();
        }
    }
}
