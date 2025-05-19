using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expValue = 10; // Her pickup ne kadar exp verir?
    public static float magnetRange = 5f;         // Oyuncuya yakla�ma mesafesi
    public float moveSpeed = 5f;           // Yakla�ma h�z�
    public float destroyTime=0;
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
        Destroy(gameObject, destroyTime);   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddExp(expValue);
            Destroy(gameObject);  
        }
    }
}
