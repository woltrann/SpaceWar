using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Enemy bossEnemy;         // Inspector’dan atayacaksýn
    public Slider healthSlider;     // Inspector’dan atayacaksýn

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
