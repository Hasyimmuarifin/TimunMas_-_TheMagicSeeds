using UnityEngine;

public class EnemyProjectile2 : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    public int damage = 25;

    void Start()
    {
        // Cari GameObject dengan tag "Player"
        GameObject target = GameObject.FindGameObjectWithTag("Buto");
        if (target != null)
        {
            // Hitung arah dari posisi proyektil ke target
            Vector2 targetPosition = target.transform.position;
            Vector2 currentPosition = transform.position;
            direction = (targetPosition - currentPosition).normalized;
        }
        else
        {
            // Jika tidak ketemu, default ke kiri
            direction = Vector2.left;
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Buto"))
        {
            other.GetComponent<ButoIjoHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}