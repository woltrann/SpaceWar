using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;  // Takip edilecek obje
    [SerializeField] private Vector3 offset;    // Kamera ile hedef arasýndaki mesafe

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }

    // Kamera hedefini dinamik olarak atamak için public fonksiyon
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
