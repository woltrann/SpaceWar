using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

public class ShipButtonUIController : MonoBehaviour
{
    public LocalizedString buyText;
    public LocalizedString selectText;
    public LocalizedString selectedText;

    public static ShipButtonUIController Instance;
    public Text actionButtonText;
    public Button actionButton;

    private ShipDetails currentShip;

    public GameObject[] shipPrefabs; // 0 -> Gemi1, 1 -> Gemi2, ...
    private GameObject currentSpawnedShip;
    public Transform shipParent;

    public Button upgradeButton;
    public Text upgradeButtonText;

    private int saveShipLevel;
    private float saveattackPower;
    private float saveattackSpeed;
    private float savemoveSpeed;
    private float saveHealth;
    private float saveShield;
    private float saveattackRange;


    private void Start()
    {
        Instance = this;
        if (GameManager.Instance.selectedShip != null)
        {
            currentSpawnedShip = Instantiate(GameManager.Instance.selectedShip.prefab, shipParent);
            currentSpawnedShip.transform.localPosition = Vector3.zero;
            Camera.main.GetComponent<CameraFollow>().SetTarget(currentSpawnedShip.transform);
        }
    }


    public void UpdateButton(ShipDetails newShip)
    {
        currentShip = newShip;


        if (GameManager.Instance.ownedShips.Contains(currentShip))
        {
            if (GameManager.Instance.selectedShip == currentShip)
            {
                actionButtonText.text = selectedText.GetLocalizedString(); // "Se�ildi"
                actionButton.interactable = false;
                Debug.Log("Se�ildi");
            }
            else
            {
                actionButtonText.text = selectText.GetLocalizedString();   // "Se�"
                actionButton.interactable = true;
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() => SelectShip(currentShip));
            }

            // Upgrade butonu aktif
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.onClick.RemoveAllListeners();

            if (currentShip.shipLevel < 50)
            {
                upgradeButtonText.text = $"({currentShip.GetUpgradeCost()} G)";
                upgradeButton.onClick.AddListener(() => UpgradeShip(currentShip));
                upgradeButton.interactable = GameManager.Instance.totalGold >= currentShip.GetUpgradeCost();
            }
            else
            {
                upgradeButtonText.text = "MAX";
                upgradeButton.interactable = false;
            }
        }
        else
        {
            actionButtonText.text = $"{buyText.GetLocalizedString()} ({currentShip.price}G)"; // "Sat�n Al"
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => BuyShip(currentShip));
            upgradeButton.gameObject.SetActive(false);

            actionButton.interactable = GameManager.Instance.totalGold >= currentShip.price;
        }
    }
    private void UpgradeShip(ShipDetails ship)
    {
        int cost = ship.GetUpgradeCost();

        if (GameManager.Instance.totalGold >= cost)
        {
            GameManager.Instance.totalGold -= cost;
            GameManager.Instance.TotalGoldText.text = GameManager.Instance.totalGold.ToString();
            PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);

            upgradeButtonText.text =cost.ToString();
            if (ship.shipLevel < 50) ship.shipLevel++;
            if (ship.attackPower < 100) ship.attackPower += 2f;
            if (ship.attackSpeed < 50) ship.attackSpeed += 2f; // �rnek art��
            if (ship.moveSpeed < 25) ship.moveSpeed += 1f;
            if (ship.health < 200) ship.health += 5f;
            if (ship.shield < 200) ship.shield += 5f; // Ornek
            if (ship.attackRange < 60) ship.attackRange += 1f;

            int shipIndex = GameManager.Instance.allShips.IndexOf(ship);
            if (shipIndex != -1)
            {
                PlayerPrefs.SetInt("Ship" + shipIndex + "_Level", ship.shipLevel);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Attack", ship.attackPower);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Speed", ship.attackSpeed);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Move", ship.moveSpeed);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Health", ship.health);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Shield", ship.shield);
                PlayerPrefs.SetFloat("Ship" + shipIndex + "_Range", ship.attackRange);
            }

            HangarController.Instance.UpdateShipDisplay();
            Debug.Log($"{ship.shipName} upgrade edildi! Level: {ship.shipLevel}");

            // Sahnedeki gemiyi g�ncelle
            if (currentSpawnedShip != null)
            {
                PlayerSmoothFollow shipController = currentSpawnedShip.GetComponent<PlayerSmoothFollow>();
                if (shipController != null)
                {
                    shipController.ApplyStats(ship); // Bu fonksiyonu birazdan yaz�yoruz
                }
            }

            // Buton g�ncelle
            UpdateButton(ship);
        }
        else
        {
            Debug.Log("Upgrade i�in yeterli alt�n yok!");
        }
    }
    private void SelectShip(ShipDetails ship)
    {
        GameManager.Instance.selectedShip = ship;
        Debug.Log($"Se�ilen gemi: {ship.shipName}");
        // ship'in index'ini bul
        int index = GameManager.Instance.allShips.IndexOf(ship);
        PlayerPrefs.SetInt("SelectedShipIndex", index);
        PlayerPrefs.Save();

        // �nce varsa eski gemiyi yok et
        if (currentSpawnedShip != null)
        {
            Destroy(currentSpawnedShip);
        }

        // Gemi prefab'�n� instantiate et
        if (ship.prefab != null)
        {
            currentSpawnedShip = Instantiate(ship.prefab, shipParent);
            currentSpawnedShip.transform.localPosition = Vector3.zero; // Ortala
            Camera.main.GetComponent<CameraFollow>().SetTarget(currentSpawnedShip.transform);

        }
        else
        {
            Debug.LogWarning("Ship prefab atanmam��!");
        }
        UpdateButton(ship);
    } 
    private void BuyShip(ShipDetails ship)
    {
        if (GameManager.Instance.totalGold >= ship.price)
        {
            GameManager.Instance.totalGold -= ship.price;
            GameManager.Instance.TotalGoldText.text=GameManager.Instance.totalGold.ToString();
            PlayerPrefs.SetFloat("TotalGold", GameManager.Instance.totalGold);

            int index = GameManager.Instance.allShips.IndexOf(ship);
            if (index >= 0)
            {
                PlayerPrefs.SetInt("Ship" + index, 1); // Bu gemi sat�n al�nd�
                PlayerPrefs.Save();
            }

            GameManager.Instance.ownedShips.Add(ship);

            Debug.Log($"Sat�n al�nd�: {ship.shipName}");
            UpdateButton(ship); // Butonu "Se�" olarak g�ncelle
        }
        else
        {
            Debug.Log("Yetersiz bakiye!");
        }
    }
    public void ResetBuyingShip()
    {
        PlayerPrefs.DeleteKey("Ship0");
        PlayerPrefs.DeleteKey("Ship1");
        PlayerPrefs.DeleteKey("Ship2");
        PlayerPrefs.DeleteKey("Ship3");
        PlayerPrefs.DeleteKey("Ship4");
        PlayerPrefs.DeleteKey("Ship5");
        PlayerPrefs.SetInt("SelectedShipIndex", 0);
        PlayerPrefs.Save();
    }


}
