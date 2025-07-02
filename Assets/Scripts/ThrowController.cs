using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwCooldown = 1f; // Waktu jeda antar lemparan (detik)
    private float lastThrowTime = -Mathf.Infinity;

    private Animator animator;
    private PlayerMovement playerMovement; // Referensi ke script PlayerMovement

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // Ambil komponen di GameObject yang sama
    }
    void Update()
    {
        // Cek apakah player sudah mati dari PlayerMovement
        if (playerMovement != null && playerMovement.isDead) return;

        if (Input.GetKeyDown(KeyCode.F) && Time.time >= lastThrowTime + throwCooldown)
        {
            Throw();
            // Aktifkan animasi lempar
            animator.SetTrigger("Throw");
            lastThrowTime = Time.time;
        }
    }

    void Throw()
    {
        GameObject proj = Instantiate(projectilePrefab, throwPoint.position, throwPoint.rotation);
        // Tentukan arah berdasarkan scale X karakter (umumnya jadi penanda arah hadap)
        Vector2 arah = transform.localScale.x < 0 ? Vector2.left : Vector2.right;

        // Set arah pada Projectile
        proj.GetComponent<Projectile>().SetDirection(arah);
        }
}