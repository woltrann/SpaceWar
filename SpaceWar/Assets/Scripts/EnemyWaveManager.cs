using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Buraya yaz�yoruz
[System.Serializable]
public class EnemyWave 
{
    public GameObject enemyPrefab;
    public float startTime = 10f; // Ne zaman ��kmaya ba�las�n?
    public float initialSpawnInterval = 5f; // Ba�ta ne kadar s�rede bir spawn
    public float minSpawnInterval = 1f; // En fazla ne kadar h�zlans�n
    public float spawnSpeedUpRate = 0.1f; // Ne kadar s�rede bir h�zlans�n
}
[System.Serializable]
public class EnemyWaveSet
{
    public int level; // Bu set hangi levelde aktif olacak?
    public List<EnemyWave> enemies;
}

public class EnemyWaveManager : MonoBehaviour
{
    public List<EnemyWaveSet> levelWaves;
    private int currentLevelforEnemy = 1;
    public Transform playerTransform; // Uzay gemisinin transformu
    public float spawnRadius = 50f; // Oyuncu etraf�nda spawn yar��ap�
    private int lastLevel = -1;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private void Start()
    {
        StartLevel(GameManager.currentLevel);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        currentLevelforEnemy = GameManager.currentLevel;
        int currentLevel = GameManager.currentLevel;

        if (currentLevel != lastLevel)
        {
            lastLevel = currentLevel;
            StopAllWaves();
            StartLevel(currentLevel);
        }
    }

    public void StartLevel(int level)
    {
        // currentLevel'den k���k veya e�it olan en b�y�k level setini bul
        EnemyWaveSet waveSet = null;
        int maxValidLevel = int.MinValue;

        foreach (var set in levelWaves)
        {
            if (set.level <= level && set.level > maxValidLevel)
            {
                maxValidLevel = set.level;
                waveSet = set;
            }
        }

        if (waveSet == null)
        {
            Debug.LogWarning($"Level {level} i�in uygun dalga seti bulunamad�!");
            return;
        }

        foreach (EnemyWave wave in waveSet.enemies)
        {
            Coroutine c = StartCoroutine(SpawnWave(wave));
            activeCoroutines.Add(c);
        }
    }
    private IEnumerator SpawnWave(EnemyWave data)
    {
        yield return new WaitForSeconds(data.startTime);

        float interval = data.initialSpawnInterval;

        while (true)
        {
            SpawnEnemy(data.enemyPrefab);
            yield return new WaitForSeconds(interval);
            if (interval > data.minSpawnInterval)    
                interval -= data.spawnSpeedUpRate;           
        }
    }

    private void StopAllWaves()
    {
        foreach (var coroutine in activeCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }

    private void SpawnEnemy(GameObject prefab)
    {
        if (playerTransform != null)
        {
            if (SkillDropSlot.Instance.enemyMoveOff) return;

            float angle = Random.Range(0f, 360f);   // 0 ile 360 derece aras�nda rastgele bir a�� se�
            float radians = angle * Mathf.Deg2Rad;
            Vector3 spawnPosition = playerTransform.position + new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * spawnRadius;   // Bu a��ya g�re X ve Z koordinatlar�n� hesapla  
            spawnPosition.y = playerTransform.position.y;    // Y�ksekli�ini sabit tutal�m (yerde do�acak mesela)
            Instantiate(prefab, spawnPosition, Quaternion.identity);   // D��man� olu�tur
        }
        
    }
}

