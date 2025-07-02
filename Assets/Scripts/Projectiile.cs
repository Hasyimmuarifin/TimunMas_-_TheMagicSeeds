using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 5;
    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 2f); // Hancurkan otomatis setelah 2 detik
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // Jika perlu, flip sprite berdasarkan arah
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Buto"))
        {
            // Kurangi HP Buto Ijo
            ButoIjoHealth butoHealth = collision.collider.GetComponentInParent<ButoIjoHealth>();
            if (butoHealth != null)
            {
                butoHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}