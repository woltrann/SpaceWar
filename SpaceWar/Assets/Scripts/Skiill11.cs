using System.Collections;
using UnityEngine;

public class Skill11 : MonoBehaviour
{
    private float radius = 15f;
    private float pullSpeed = 10f;
    private float duration = 5f;

    private void Start()
    {
        StartCoroutine(PullCoroutine());
    }

    private IEnumerator PullCoroutine()
    {
        float timer = 0f;
        while (timer < duration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 targetPoint = transform.position + Vector3.up * 4f; // Mesela merkezin 2 birim �st�
                        Vector3 direction = (targetPoint - col.transform.position).normalized;
                        rb.linearVelocity = direction * pullSpeed;  // Kuvvet de�il, sabit h�z
                    }
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // �ekim bittikten sonra etkilenen t�m d��manlar�n h�z�n� s�f�rla
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in affectedColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
