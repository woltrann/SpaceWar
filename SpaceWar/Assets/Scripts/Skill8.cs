using System.Linq;
using UnityEngine;

public class Skill8 : MonoBehaviour
{
    public float followDistance = 3f;                  // Player'a yaklaşma mesafesi
    public float moveSpeed = 11f;                      // Klonun hareket hızı
    public float fireRate = 1f;                        // Atış süresi
    public GameObject bulletPrefab;                    // Klonun ateşleyeceği mermi
    public Transform firePoint;                        // Merminin çıkacağı yer
    public float cloneLifetime = 10f;                  // Yaşam süresi
    private float fireCooldown = 0f;                   // Ateş süresi için zamanlayıcı

    private Transform player;
    private float lockOnRange = 30f;
    private float lockOnTurnSpeed = 250f;
    private Transform nearestEnemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, cloneLifetime);
    }

    void Update()
    {
        if (player == null) return;

        // Hedef pozisyon: Oyuncunun arkasında dur
        Vector3 targetPosition = player.position - player.forward * followDistance;

        // Pozisyona yumuşak hareket
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Hedef bul ve ateş et
        FindNearestEnemyAndLockOn();

        // Eğer düşman yoksa, hareket yönüne göre bakış yönünü ayarla
        if (nearestEnemy == null)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
            }
        }
        else
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireAtEnemy();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    private void FindNearestEnemyAndLockOn()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance && distance <= lockOnRange)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            nearestEnemy = closestEnemy;

            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTurnSpeed * Time.deltaTime);
        }
        else
        {
            nearestEnemy = null;
        }
    }

    private void FireAtEnemy()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Bullet scriptine hasar gönder
            Bullet bulletScript = bulletGO.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(PlayerSmoothFollow.Instance.damage / 2);
            }
        }
    }
}
