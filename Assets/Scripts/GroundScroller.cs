using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GroundScroller : MonoBehaviour
{
    [Header("Scrolling Settings")]
    public float speed = 2f;
    public float resetX = 60f;
    public float stopAtX = -20f;

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "kalah";
    [SerializeField] private int maxLoops = 5;
    [SerializeField] private bool isCounter = false;

    [Header("UI Settings")]
    public GameObject panelKonfirmasi;

    [Header("Transisi Animasi")]
    public Animator butoAnimator;
    public GameObject[] awanImages;
    public CanvasGroup[] awanCanvasGroups;
    public GameObject logoImage;
    public CanvasGroup logoCanvasGroup;

    [Header("Delay Transisi")]
    public float delaySebelumAwan = 2f;
    public float delayAntarAwan = 0.5f;
    public float delayLogo = 1f;
    public float delaySebelumLoad = 2f;
    public float durasiFade = 1f;

    [Header("Audio Settings")]
    public AudioSource backsoundSource;
    public AudioSource butoAudioSource;  // <--- suara buto

    private static int loopCount = 0;
    private static bool hasTriggeredPanel = false;

    void Start()
    {
        // mulai suara buto dari awal, looping
        if (butoAudioSource != null)
        {
            butoAudioSource.loop = true;
            butoAudioSource.Play();
        }
    }

    void Update()
    {
        if (hasTriggeredPanel) return;

        // Geser objek ke kiri
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= stopAtX)
        {
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);

            if (isCounter)
            {
                loopCount++;
                Debug.Log($"Loop ke-{loopCount}");

                if (loopCount >= maxLoops)
                {
                    hasTriggeredPanel = true;

                    // hentikan suara buto
                    if (butoAudioSource != null)
                    {
                        butoAudioSource.Stop();
                    }

                    // 1. Ubah animasi buto ke Idle
                    if (butoAnimator != null)
                    {
                        butoAnimator.SetTrigger("ToIdle");
                    }

                    // 2. Tampilkan panel konfirmasi
                    if (panelKonfirmasi != null)
                    {
                        panelKonfirmasi.SetActive(true);
                    }

                    // 3. Mulai transisi awan & logo
                    StartCoroutine(TransisiAwan());
                }
            }
        }
    }

    IEnumerator TransisiAwan()
    {
        yield return new WaitForSeconds(delaySebelumAwan);

        for (int i = 0; i < awanImages.Length; i++)
        {
            if (awanImages[i] != null && awanCanvasGroups[i] != null)
            {
                awanImages[i].SetActive(true);
                yield return StartCoroutine(FadeCanvasGroup(awanCanvasGroups[i], 0f, 1f, durasiFade));
                yield return new WaitForSeconds(delayAntarAwan);
            }
        }

        yield return new WaitForSeconds(delayLogo);

        if (logoImage != null && logoCanvasGroup != null)
        {
            logoImage.SetActive(true);

            if (backsoundSource != null)
            {
                backsoundSource.Play();
            }

            yield return StartCoroutine(FadeCanvasGroup(logoCanvasGroup, 0f, 1f, durasiFade));
        }

        yield return new WaitForSeconds(delaySebelumLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        cg.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
    }

    public void LanjutkanKeSceneBerikutnya()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
