using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Localization.Platform.Android;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSmoothFollow : MonoBehaviour
{
    public ShipDetails stats;
    private ShipDetails runtimeStats;

    public static bool fight=false;

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip fireSound;


    [Header("Tilt Settings")]
    public float tiltAmount = 20f;
    public float tiltSpeed = 30f;

    [Header("Ground Expansion")]
    private GameObject planeReference;
    private GameObject player;
    public float planeSize = 500f; // Plane boyutu (1 birim = 1 scale ise 10f olur)
    private HashSet<Vector3> spawnedDirections = new HashSet<Vector3>();
    private List<GameObject> allPlanes = new List<GameObject>();
    private List<Vector3> spawnedPlanePositions = new List<Vector3>();

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float moveSpeed;
    private float fireRate;
    private float nextFireTime = 0f;
    private float Health;
    private float shield;
    private Slider healthSlider;

    [Header("Hit Effect")]
    public GameObject hitParticle;
    public Joystick joystick;

    [Header("Target Lock Settings")]
    private float lockOnRange; // Kaç birim menzilde düşmanlara bakacak
    private float lockOnTurnSpeed = 100f; // Düşmana kilitleninceki dönüş hızı
    private Transform nearestEnemy;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        planeReference = GameObject.FindGameObjectWithTag("Plane");
        GameObject initialPlane = GameObject.FindGameObjectWithTag("Plane");
        if (initialPlane != null)
        {
            planeReference = initialPlane;
            allPlanes.Add(planeReference);
        }

        healthSlider = GameObject.Find("Canvas/GUX/Image/HealthBar/HealthSlider").GetComponent<Slider>();
        joystick = GameObject.Find("Canvas/GUX/Joy").GetComponent<Joystick>();

        runtimeStats = ScriptableObject.CreateInstance<ShipDetails>();
        DeletableStat();
        CalculateLimits();
    }
    private void Update()
    {
        moveSpeed = runtimeStats.moveSpeed;

        Move();
        if(fight)Fire();
        //FindNearestEnemyAndLockOn(); // Her frame kontrol et
        CheckAllPlaneEdges();
    }


    private void Move()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

            transform.position = targetPosition; // Sınırsız hareket

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
        }
    }
    private void Fire()
    {
        //Input.touchCount > 0 &&
        if ( Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(runtimeStats.attackPower);
            }
            nextFireTime = Time.time + (10/ runtimeStats.attackSpeed);
        }
    }
    public void TakeDamage(float amount)
    {
        Health -= amount;
        Health = Mathf.Clamp(Health, 0, runtimeStats.health);
        healthSlider.value = Health;

        if (Health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

        if (hitParticle != null)
        {
            GameObject particleEffect = Instantiate(hitParticle, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                
            }
            Destroy(particleEffect, 2f);
        }
        Destroy(gameObject);

        GameManager.Instance.GameOver();
    }

    private void CalculateLimits()
    {
        float distanceFromCamera = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);

        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceFromCamera));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceFromCamera));

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

            // Hızlı dönüş için lockOnTurnSpeed kullan
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTurnSpeed * Time.deltaTime);
        }
        else
        {
            nearestEnemy = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Enemy"))
        {
            transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

            if (hitParticle != null)
            {
                GameObject particleEffect = Instantiate(hitParticle, transform.position, Quaternion.identity);
                ParticleSystem particleSystem = particleEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
                Destroy(particleEffect, 2f);
            }
            Destroy(gameObject); 
            
            GameManager.Instance.GameOver();
        }
    }
    public void ApplyStats(ShipDetails stats)
    {
        Health = stats.health;
        healthSlider.maxValue = stats.health;
        healthSlider.value = Health;

        shield = stats.shield;
        lockOnRange = stats.attackRange;
        moveSpeed = stats.moveSpeed;
        fireRate = stats.attackSpeed;
        // gerekiyorsa diğer özellikleri de ata
        DeletableStat();
    }
    public void DeletableStat()
    {
        runtimeStats.health = stats.health;
        runtimeStats.moveSpeed = stats.moveSpeed;
        runtimeStats.attackPower = stats.attackPower;
        runtimeStats.attackSpeed = stats.attackSpeed;
        runtimeStats.attackRange = stats.attackRange;

        Health = runtimeStats.health;
        healthSlider.maxValue = runtimeStats.health;
        healthSlider.value = Health;

        lockOnRange = runtimeStats.attackRange;
        moveSpeed = runtimeStats.moveSpeed;
        fireRate = runtimeStats.attackSpeed;
    }
    public void ApplyCard(CardDetails card)
    {
        // Sağlık
        Health += card.cardHealth;
        Health = Mathf.Clamp(Health, 0, runtimeStats.health + card.cardHealth);
        healthSlider.maxValue = runtimeStats.health;
        healthSlider.value = Health;

        runtimeStats.attackPower += card.cardAttack;
        runtimeStats.attackSpeed += card.cardAttackSpeed;
        runtimeStats.moveSpeed += card.cardShipSpeed;
        runtimeStats.attackRange += card.cardAttackRange;

        // Magnet Alanı (İleride bir scriptte kullanıyorsan)
        GoldPickup.magnetRange += card.cardMagnetArea;
        ExpPickup.magnetRange += card.cardMagnetArea;

        Debug.Log($"Uygulanan Kart: {card.cardName}");
        Debug.Log("attack:" + runtimeStats.attackPower + "-Attack speed:" + runtimeStats.attackSpeed+"-Ship speed:"+ runtimeStats.moveSpeed+"-Range:"+ runtimeStats.attackRange+"-health:"+ Health);
    }


    private void CheckAllPlaneEdges()
    {
        List<GameObject> newPlanes = new List<GameObject>();

        foreach (GameObject plane in allPlanes)
        {
            Vector3 planePos = plane.transform.position;

            // Plane’in 4 yönü
            Vector3[] directions = new Vector3[]
            {
                Vector3.forward * planeSize,
                Vector3.back * planeSize,
                Vector3.left * planeSize,
                Vector3.right * planeSize
            };

            foreach (Vector3 dir in directions)
            {
                Vector3 newPos = planePos + dir;

                if (!spawnedPlanePositions.Contains(newPos))
                {
                    // Eğer oyuncu bu yeni pozisyona yakınsa spawnla
                    float distanceToPlayer = Vector3.Distance(player.transform.position, newPos);
                    if (distanceToPlayer <= planeSize * 1.5f)
                    {
                        GameObject newPlane = Instantiate(planeReference, newPos, Quaternion.identity);
                        newPlanes.Add(newPlane);
                        spawnedPlanePositions.Add(newPos);
                    }
                }
            }
        }

        // Yeni oluşanları ana listeye ekle
        allPlanes.AddRange(newPlanes);
    }

    private void SpawnNewPlaneAt(GameObject basePlane, Vector3 direction)
    {
        Vector3 planeSize = new Vector3(
            10 * basePlane.transform.localScale.x,
            0f,
            10 * basePlane.transform.localScale.z
        );

        Vector3 spawnPos = basePlane.transform.position + Vector3.Scale(direction, planeSize);

        if (spawnedPlanePositions.Contains(spawnPos)) return;

        GameObject newPlane = Instantiate(planeReference, spawnPos, basePlane.transform.rotation);
        newPlane.tag = "Untagged";
        spawnedPlanePositions.Add(spawnPos);
        allPlanes.Add(newPlane);
    }


}
