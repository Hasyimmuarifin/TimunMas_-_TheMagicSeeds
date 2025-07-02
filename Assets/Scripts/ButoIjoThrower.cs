using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButoIjoThrower : MonoBehaviour
{
    public GameObject[] projectilePrefabs;      // Array prefab proyektil acak
    public Transform throwPoint;                // Titik lemparan
    public float minThrowInterval = 2f;
    public float maxThrowInterval = 5f;
    public bool isDead = false;
    private Rigidbody2D body;
    private Animator animator;

    [Header("Audio Settings")]
    public AudioSource jumpAudioSource;
    public AudioSource crashAudioSource;
    private bool isThrowing = false;
    public string sceneMenang;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ThrowLoop());
        body = GetComponent<Rigidbody2D>();
    }

    IEnumerator ThrowLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minThrowInterval, maxThrowInterval);
            yield return new WaitForSeconds(waitTime);

            // Aktifkan animasi lempar
            animator.SetTrigger("Throw");

            // Tunggu sedikit agar sinkron dengan animasi lempar
            yield return new WaitForSeconds(0.3f);

            // Aktifkan animasi lempar
            animator.SetTrigger("ToIdle");

            ThrowProjectile();
        }
    }

    void ThrowProjectile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        GameObject selectedProjectile = projectilePrefabs[index];

        Instantiate(selectedProjectile, throwPoint.position, throwPoint.rotation);
    }
    public void TriggerDeathButo()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        Debug.Log("Buto mati karena HP habis.");

        body.linearVelocity = Vector2.zero;
        body.isKinematic = true;

        StartCoroutine(PlayCrashThenLoadSceneMenang());
    }
    IEnumerator PlayCrashThenLoadSceneMenang()
    {
        if (crashAudioSource != null)
        {
            crashAudioSource.Play();
            yield return new WaitForSeconds(crashAudioSource.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }

        SceneManager.LoadScene(sceneMenang);
    }
}