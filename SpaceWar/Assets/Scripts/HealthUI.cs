using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Enemy EnemyBoss;         // Inspector�dan atayacaks�n
    public Slider healthSlider;     // Inspector�dan atayacaks�n

    void Start()
    {
        if (EnemyBoss != null && healthSlider != null)
        {
            healthSlider.maxValue = EnemyBoss.maxHealth;
            healthSlider.value = EnemyBoss.GetCurrentHealth();
        }
    }

    void Update()
    {
        if (EnemyBoss != null && healthSlider != null)
        {
            healthSlider.value = EnemyBoss.GetCurrentHealth();
        }
    }
}
