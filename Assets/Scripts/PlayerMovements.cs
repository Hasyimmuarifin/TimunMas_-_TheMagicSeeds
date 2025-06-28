using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioSource jumpAudioSource;   // <--- ini tambahan

    private Rigidbody2D body;
    private Animator anim;

    private float runTimer = 0f;
    private bool wasRunning = false;
    private bool isGrounded = false;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("Run", false);
        anim.SetBool("IdleAfterRun", false);
    }

    void Update()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * groundCheckYOffset;
        isGrounded = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, groundCheckRadius);
        foreach (var col in hits)
            if (col != null && col.CompareTag("Ground"))
            {
                isGrounded = true;
                break;
            }
        anim.SetBool("Grounded", isGrounded);

        if (Input.GetKeyDown(throwKey) && isGrounded)
        {
            anim.SetTrigger("Throw");
            return;
        }

        float h = Input.GetAxis("Horizontal");
        bool isRunning = Mathf.Abs(h) > 0.1f;
        body.linearVelocity = new Vector2(h * speed, body.linearVelocity.y);

        if (h > 0.01f) transform.localScale = Vector3.one;
        else if (h < -0.01f) transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");

            if (jumpAudioSource != null)
                jumpAudioSource.Play();   // <--- mainkan sfx jump

            return;
        }

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
        if (collision.gameObject.CompareTag("Buto") || collision.gameObject.CompareTag("rintangan"))
        {
            Debug.Log("Bertabrakan dengan " + collision.gameObject.tag + ", pindah ke scene: " + sceneKalah);
            SceneManager.LoadScene(sceneKalah);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * groundCheckYOffset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, groundCheckRadius);
    }
}
