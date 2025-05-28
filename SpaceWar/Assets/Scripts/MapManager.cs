using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapData
{
    public string mapName;
    public Material mapMaterial;
    public Sprite previewImage;
    public Sprite lockedImage;
    public bool isUnlocked = false;
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public MapData[] maps;
    public int currentIndex = 0;

    [Header("UI Elements")]
    public Text mapNameText;
    public Image mapPreviewImage;
    public Image mapLockedOverlayImage; // 💡 Yeni overlay görseli
    public Button leftButton;
    public Button rightButton;

    [Header("Target")]
    public Renderer groundRenderer;

    void Start()
    {
        Instance = this;
        if (maps == null || maps.Length == 0)
        {
            Debug.LogWarning("Map listesi boş!");
            return;
        }
        
        for (int i = 0; i < maps.Length; i++)   //   Daha önce açılmış haritaları yükle
        {
            maps[i].isUnlocked = PlayerPrefs.GetInt($"MapUnlocked_{i}", i == 0 ? 1 : 0) == 1;
        }

        //  En son hangi harita seçildiyse onu yükle
        currentIndex = PlayerPrefs.GetInt("CurrentMapIndex", 0);
        currentIndex = Mathf.Clamp(currentIndex, 0, maps.Length - 1);

        UpdateUI();
        leftButton.onClick.AddListener(PreviousMap);
        rightButton.onClick.AddListener(NextMap);
    }

    public void UpdateUI()
    {
        if (maps.Length == 0) return;
        MapData currentMap = maps[currentIndex];
        mapNameText.text = currentMap.isUnlocked ? currentMap.mapName : "???";
        mapPreviewImage.sprite = currentMap.previewImage;        // Preview image her zaman gösterilir
        Debug.Log("1");
     
        if (mapLockedOverlayImage != null)      // Eğer kilitliyse overlay aktif edilir
        {
            mapLockedOverlayImage.gameObject.SetActive(!currentMap.isUnlocked);
            mapLockedOverlayImage.sprite = currentMap.lockedImage;
            Debug.Log("2");
        }

        if (groundRenderer != null && currentMap.isUnlocked && currentMap.mapMaterial != null)      // Yere materyali sadece açıksa uygula
        {
            groundRenderer.material = currentMap.mapMaterial;
            Debug.Log("3");
        }
        if (currentMap.isUnlocked && currentMap.mapMaterial != null)
        {
            ApplyMaterialToAllGroundRenderers(currentMap.mapMaterial);
        }
        PlayerPrefs.SetInt("CurrentMapIndex", currentIndex);
        PlayerPrefs.Save();
    }
    void ApplyMaterialToAllGroundRenderers(Material newMat)
    {
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Plane");

        foreach (GameObject obj in groundObjects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = newMat; // veya rend.sharedMaterial
            }
        }
    }

    void PreviousMap()
    {
        currentIndex = (currentIndex - 1 + maps.Length) % maps.Length;
        UpdateUI();
    }

    void NextMap()
    {
        currentIndex = (currentIndex + 1) % maps.Length;
        UpdateUI();
    }

    public void LoadNextMapAfterBoss()
    {
        if (currentIndex + 1 >= maps.Length)
        {
            Debug.Log("Tüm haritalar tamamlandı!");
            return;
        }

        currentIndex++;
        UpdateUI();
        Debug.Log("Yeni harita yüklendi: " + maps[currentIndex].mapName);
    }

    public void UnlockNextMap()
    {
        if (currentIndex + 1 < maps.Length)
        {
            maps[currentIndex + 1].isUnlocked = true;
            Debug.Log("Yeni harita açıldı: " + maps[currentIndex + 1].mapName);
        }
        if (currentIndex + 1 < maps.Length)
        {
            maps[currentIndex + 1].isUnlocked = true;
            PlayerPrefs.SetInt($"MapUnlocked_{currentIndex + 1}", 1); // 🔒 Kayıt
            PlayerPrefs.Save(); // Opsiyonel ama güvenli
            Debug.Log("Yeni harita açıldı: " + maps[currentIndex + 1].mapName);
        }
        UpdateUI();
    }
    public void ResetMapProgress()
    {
       

        // 🔒 Tüm harita kilit durumları sıfırlanır
        for (int i = 0; i < maps.Length; i++)
        {
            PlayerPrefs.DeleteKey($"MapUnlocked_{i}");
        }

        PlayerPrefs.Save();
        Debug.Log("Tüm kayıtlar sıfırlandı!");

        // Bellekteki map verilerini de sıfırla
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].isUnlocked = (i == 0); // Sadece ilk harita açık
        }

        currentIndex = 0;
        UpdateUI();
    }

}
