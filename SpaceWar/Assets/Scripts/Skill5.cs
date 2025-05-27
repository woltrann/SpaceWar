using UnityEngine;

public class Skill5 : MonoBehaviour
{
    public GameObject bulletPrefab;     // Mermi prefab�
    public Transform firePoint;         // Merminin ��kaca�� nokta
    public float fireRate = 1f;         // Saniyede 1 mermi
    private float fireCooldown = 0f;    // Ate� s�resi i�in zamanlay�c�

    private float lockOnRange=20f;
    private Transform nearestEnemy;
    private float lockOnTurnSpeed=250f;

    public float attackPower = 10f; // Bunu gemiden alacaks�n ya da elle ayarla


    void Update()
    {
        FindNearestEnemyAndLockOn();
        if (nearestEnemy != null)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireAtEnemy();
                fireCooldown = 1f / fireRate;
            }
        }
        Destroy(gameObject,7f);
    }
    private void FireAtEnemy()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Bullet scriptine hasar g�nder
            Bullet bulletScript = bulletGO.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(PlayerSmoothFollow.Instance.damage/2); // veya bulletScript.SetDamage(runtimeStats.attackPower);
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
}
