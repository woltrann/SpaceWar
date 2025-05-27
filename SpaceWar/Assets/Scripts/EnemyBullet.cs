using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public int attackDamage = 10;

    private void Update()
    {    
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);  // Mermi ileri do�ru (ekran�n alt�na do�ru) gider
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerSmoothFollow player = other.GetComponent<PlayerSmoothFollow>();
            if (player != null)
            {
                player.TakeDamage(attackDamage); // 10 hasar verecek, istersen de�i�tir.
            }

            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("ShieldArea"))
        {
            Destroy(gameObject);
        }

    }
}
