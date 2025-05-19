using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab;     // Zemin prefabı
    public float speed = 10f;            // Zeminin kayma hızı
    public int numberOfGrounds = 3;      // Kaç tane zemin başta spawnlansın
    public float groundLength = 300f;    // Her zeminin uzunluğu (scale'a göre)
    public Transform player;             // Karakter transformu (şu an sabit)

    private List<GameObject> activeGrounds = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < numberOfGrounds; i++)
        {
            SpawnGround(i * groundLength);
        }
    }

    private void Update()
    {
        MoveGrounds();

        // Eğer en öndeki zemin ekran dışına çıktıysa
        if (activeGrounds[0].transform.position.z < player.position.z - groundLength)
        {
            Destroy(activeGrounds[0]);
            activeGrounds.RemoveAt(0);

            // En arkaya yeni bir zemin spawnla
            float newZ = activeGrounds[activeGrounds.Count - 1].transform.position.z + groundLength;
            SpawnGround(newZ);
        }
    }

    private void MoveGrounds()
    {
        foreach (GameObject ground in activeGrounds)
        {
            ground.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void SpawnGround(float zPos)
    {
        GameObject newGround = Instantiate(groundPrefab, new Vector3(0, 0, zPos), Quaternion.identity);
        activeGrounds.Add(newGround);
    }
}
