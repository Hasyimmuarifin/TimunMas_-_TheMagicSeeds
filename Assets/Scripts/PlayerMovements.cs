using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Run Timing")]
    [SerializeField] private float longRunThreshold = 0.5f;

    [Header("Ground Check (via offset)")]
    [SerializeField] private float groundCheckYOffset = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Throw")]
    [SerializeField] private KeyCode throwKey = KeyCode.F;

    [Header("Scene Settings")]
    [SerializeField] private string sceneKalah = "kalah";

    [Header("Audio Settings")]
    public AudioSource jumpAudioSource;
    public AudioSource crashAudioSource;

    [Header("GroundScroller")]
    public GroundScroller groundScroller; // drag GroundScroller disini

    private Rigidbody2D body;
    private Animator anim;

    private float runTimer = 0f;
    private bool wasRunning = false;
    private bool isGrounded = false;
    private bool isDead = false;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("Run", false);
        anim.SetBool("IdleAfterRun", false);
    }

    void Update()
    {
        if (isDead) return;

        // Ground check
        Vector2 origin = (Vector2)transform.position + Vector2.down * groundCheckYOffset;
        isGrounded = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, groundCheckRadius);
        foreach (var col in hits)
        {
            if (col != null && col.CompareTag("Ground"))
            {
                isGrounded = true;
                break;
            }
        }
        anim.SetBool("Grounded", isGrounded);

        // Throw
        if (Input.GetKeyDown(throwKey) && isGrounded)
        {
            anim.SetTrigger("Throw");
            return;
        }

        // Move
        float h = Input.GetAxis("Horizontal");
        bool isRunning = Mathf.Abs(h) > 0.1f;
        body.linearVelocity = new Vector2(h * speed, body.linearVelocity.y); // Diubah dari linearlinearVelocity

        if (h > 0.01f) transform.localScale = Vector3.one;
        else if (h < -0.01f) transform.localScale = new Vector3(-1, 1, 1);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce); // Diubah dari linearlinearVelocity
            anim.SetTrigger("Jump");

            if (jumpAudioSource != null)
                jumpAudioSource.Play();

            return;
        }

        // Run & Idle after run
        if (isGrounded)
        {
            if (isRunning)
            {
                anim.SetBool("Run", true);
                anim.SetBool("IdleAfterRun", false);
                runTimer += Time.deltaTime;
            }
            else
            {
                anim.SetBool("Run", false);
                if (wasRunning)
                {
                    anim.SetBool("IdleAfterRun", runTimer >= longRunThreshold);
                    runTimer = 0f;
                }
            }
        }

        wasRunning = isRunning;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Buto") || collision.gameObject.CompareTag("rintangan"))
        {
            isDead = true;
            Debug.Log("Bertabrakan dengan " + collision.gameObject.tag);
            
            // --- PERUBAHAN DIMULAI DI SINI ---

            // 1. Hentikan semua gerakan player
            body.linearVelocity = Vector2.zero;
            body.isKinematic = true; // Membuat player tidak terpengaruh fisika lagi

            // 2. Memicu animasi Die
            anim.SetTrigger("Die");

            // --- PERUBAHAN SELESAI ---

            // Beri tahu groundScroller untuk berhenti
            if (groundScroller != null)
            {
                groundScroller.BerhentiKarenaTabrakan();
            }

            // Mainkan crash SFX lalu load scene
            StartCoroutine(PlayCrashThenLoadScene());
        }
    }

    IEnumerator PlayCrashThenLoadScene()
    {
        if (crashAudioSource != null)
        {
            crashAudioSource.Play();
            // Tunggu selama durasi klip audio. Ini memberi waktu bagi animasi Die untuk berjalan.
            yield return new WaitForSeconds(crashAudioSource.clip.length);
        }
        else
        {
            // Jika tidak ada audio, beri jeda standar agar animasi sempat terlihat.
            yield return new WaitForSeconds(1f); // Anda bisa sesuaikan durasi ini
        }

        SceneManager.LoadScene(sceneKalah);
    }

    void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * groundCheckYOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, groundCheckRadius);
    }
}