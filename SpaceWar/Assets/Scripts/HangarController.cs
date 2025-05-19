using UnityEngine;
using UnityEngine.UI;

public class HangarController : MonoBehaviour
{
    public static HangarController Instance;
    public GameObject[] shipPrefabs;
    public Transform spawnParent; // CurrentShip objesi
    private int currentIndex = 0;
    private GameObject currentShip;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider attackSlider;
    public Slider speedSlider;
    public Slider shieldSlider;
    public Slider attackSpeedSlider;
    public Slider attackRangeSlider;
    public Text shipNameText;
    public Text price;
    public Text upgradeCost;

    public ShipButtonUIController buttonUI; // Inspector'da atamayý unutma


    private void Start()
    {
        Instance = this;
        UpdateShipDisplay();
    }
    public void ShowNext()
    {
        currentIndex = (currentIndex + 1) % shipPrefabs.Length;
        UpdateShipDisplay();
    }
    public void ShowPrevious()
    {
        currentIndex = (currentIndex - 1 + shipPrefabs.Length) % shipPrefabs.Length;
        UpdateShipDisplay();
    }

    public void UpdateShipDisplay()
    {
        if (currentShip != null)
            Destroy(currentShip);

        currentShip = Instantiate(shipPrefabs[currentIndex], spawnParent);
        currentShip.transform.localPosition = Vector3.zero;
        currentShip.transform.localRotation = Quaternion.Euler(0, 0, 0); // varsa özel açý
        ShipDisplay display = currentShip.GetComponent<ShipDisplay>();
        if (display != null && display.stats != null)
        {
            healthSlider.value = display.stats.health;
            attackSlider.value = display.stats.attackPower;
            speedSlider.value = display.stats.moveSpeed;
            shieldSlider.value = display.stats.shield;
            attackSpeedSlider.value = display.stats.attackSpeed;
            attackRangeSlider.value = display.stats.attackRange;
            shipNameText.text = display.stats.shipName;
            price.text = display.stats.price.ToString();
            upgradeCost.text = display.stats.upgradeCost.ToString();

            buttonUI.UpdateButton(display.stats);

        }
    }

    
}
