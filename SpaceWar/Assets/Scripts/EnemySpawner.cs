using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Spawnlanacak d��man prefab�
    public Transform playerTransform; // Uzay gemisinin transformu
    public float spawnRadius = 10f; // Oyuncu etraf�nda spawn yar��ap�
    public float spawnInterval = 2f; // Ka� saniyede bir spawn olacak

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (playerTransform == null) return;   
        float angle = Random.Range(0f, 360f);   // 0 ile 360 derece aras�nda rastgele bir a�� se�
        float radians = angle * Mathf.Deg2Rad; 
        Vector3 spawnPosition = playerTransform.position + new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * spawnRadius;   // Bu a��ya g�re X ve Z koordinatlar�n� hesapla  
        spawnPosition.y = playerTransform.position.y;    // Y�ksekli�ini sabit tutal�m (yerde do�acak mesela)
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);   // D��man� olu�tur
    }
}
