using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;

    void Start()
    {
        // Cari GameObject dengan tag "Player"
        GameObject target = GameObject.FindGameObjectWithTag("Player");
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Kurangi HP Timun Mas
            PlayerHealth playerHealth = collision.collider.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}