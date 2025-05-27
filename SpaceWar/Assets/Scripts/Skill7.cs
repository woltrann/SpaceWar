using UnityEngine;

public class Skill7 : MonoBehaviour
{
    public float fallSpeed = 10f;
    public GameObject vfxPrefab; // Patlama efekti prefab�
    public Transform visualChild; // K���lt�lecek child objeyi buraya ata


    private bool hasTriggeredEffect = false;

    void Update()
    {
        // A�a�� do�ru d��me
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // G�rsel child scale'ini k���ltme
        if (visualChild != null)
        {
            float shrinkSpeed = 0.1f; // saniyede ne kadar k���ls�n
            float minScale = 0.05f;  // minimum scale
            Vector3 newScale = visualChild.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
            visualChild.localScale = Vector3.Max(newScale, Vector3.one * minScale);
        }

        // Y�ksekli�i kontrol et
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
