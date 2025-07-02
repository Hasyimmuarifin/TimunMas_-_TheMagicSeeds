using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private PlayerHealth playerHealth;


    [Header("Run Timing")]
    [SerializeField] private float longRunThreshold = 0.5f;

    [Header("Ground Check (via offset)")]
    [SerializeField] private float groundCheckYOffset = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.1f;

    // [Header("Throw")]
    // [SerializeField] private KeyCode throwKey = KeyCode.F;

    [Header("Scene Settings")]
    [SerializeField] private string sceneKalah = "kalah";

    [Header("Audio Settings")]
    public AudioSource jumpAudioSource;
    public AudioSource crashAudioSource;

    [Header("GroundScroller")]
    public GroundScroller groundScroller;

    private Rigidbody2D body;
    private Animator anim;

    private float runTimer = 0f;
    private bool wasRunning = false;
    private bool isGrounded = false;
    public bool isDead = false;
    private bool isGameOver = false;

    // === tambahan baru
    private bool awaitLanding = false;
    private bool disableAfterLanding = false;

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

        // jika sedang menunggu landing
        if (awaitLanding && isGrounded)
        {
            // stop total
            disableAfterLanding = true;
            awaitLanding = false;
            MatikanInput();
        }

        if (isGameOver) return;

        // // Throw
        // if (Input.GetKeyDown(throwKey) && isGrounded)
        // {
        //     anim.SetTrigger("Throw");
        //     return;
        // }

        // Move
        float h = Input.GetAxis("Horizontal");
        bool isRunning = Mathf.Abs(h) > 0.1f;
        body.linearVelocity = new Vector2(h * speed, body.linearVelocity.y);

        if (h > 0.01f) transform.localScale = Vector3.one;
        else if (h < -0.01f) transform.localScale = new Vector3(-1, 1, 1);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
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

            body.linearVelocity = Vector2.zero;
            body.isKinematic = true;
            anim.SetTrigger("Die");

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(8);
            }
            else
            {
                Debug.LogWarning("PlayerHealth reference belum di-assign di Inspector.");
            }

            if (groundScroller != null)
                groundScroller.BerhentiKarenaTabrakan();

            StartCoroutine(PlayCrashThenLoadScene());
        }
    }

    public void TriggerDeath()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player mati karena HP habis.");

        body.linearVelocity = Vector2.zero;
        body.isKinematic = true;

        anim.SetTrigger("Die");

        if (groundScroller != null)
            groundScroller.BerhentiKarenaTabrakan();

        StartCoroutine(PlayCrashThenLoadScene());
    }

    IEnumerator PlayCrashThenLoadScene()
    {
        if (crashAudioSource != null)
        {
            crashAudioSource.Play();
            yield return new WaitForSeconds(crashAudioSource.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene(sceneKalah);
    }

    public void MatikanInput()
    {
        isGameOver = true;
        body.linearVelocity = Vector2.zero;
        body.isKinematic = true;
        anim.SetBool("Run", false);
        anim.SetBool("IdleAfterRun", false);
    }

    // metode baru
    public void AwaitLandingThenStop()
    {
        if (isGrounded)
        {
            MatikanInput();
        }
        else
        {
            awaitLanding = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * groundCheckYOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, groundCheckRadius);
    }
}
