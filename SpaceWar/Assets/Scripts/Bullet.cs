using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private float damage;



    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetDamage()
    {
        return damage;
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 2f); // 5 saniye sonra mermiyi yok et
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
