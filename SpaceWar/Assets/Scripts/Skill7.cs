using UnityEngine;

public class Skill7 : MonoBehaviour
{
    public float fallSpeed = 10f;
    public GameObject vfxPrefab; // Patlama efekti prefabý
    public Transform visualChild; // Küçültülecek child objeyi buraya ata


    private bool hasTriggeredEffect = false;

    void Update()
    {
        // Aþaðý doðru düþme
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Görsel child scale'ini küçültme
        if (visualChild != null)
        {
            float shrinkSpeed = 0.1f; // saniyede ne kadar küçülsün
            float minScale = 0.05f;  // minimum scale
            Vector3 newScale = visualChild.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
            visualChild.localScale = Vector3.Max(newScale, Vector3.one * minScale);
        }

        // Yüksekliði kontrol et
        if (!hasTriggeredEffect && transform.position.y <= 5f)
        {
            TriggerEffect();
        }
        Destroy(gameObject, 1.5f);
    }

    void TriggerEffect()
    {
        hasTriggeredEffect = true;

        if (vfxPrefab != null)
        {
            Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        }

    }
}
