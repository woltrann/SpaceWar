using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldValue = 10; // Her pickup ne kadar exp verir?
    public static float magnetRange = 5f;         // Oyuncuya yakla�ma mesafesi
    public float moveSpeed = 5f;           // Yakla�ma h�z�

    private Transform player;

    private void Start()
    {
       
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) < magnetRange)
            {
                // Oyuncuya do�ru hareket et
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddGold(goldValue);
            Destroy(gameObject);
        }
    }
}
