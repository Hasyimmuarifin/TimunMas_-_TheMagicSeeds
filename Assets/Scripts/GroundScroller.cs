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
    public AudioSource backsoundLogoSource;
    public AudioSource backsoundGameSource;
    public AudioSource butoAudioSource;

    private static int loopCount = 0;
    private static bool hasTriggeredPanel = false;

    void Start()
    {
        // mulai backsound game dari awal
        if (backsoundGameSource != null)
        {
            backsoundGameSource.loop = true;
            backsoundGameSource.Play();
        }

        // mulai suara buto dari awal
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
                    TriggerTransisi();
                }
            }
        }
    }

    void TriggerTransisi()
    {
        hasTriggeredPanel = true;

        // stop backsound game
        if (backsoundGameSource != null)
            backsoundGameSource.Stop();

        // stop suara buto
        if (butoAudioSource != null)
            butoAudioSource.Stop();

        // ubah animasi buto ke idle
        if (butoAnimator != null)
            butoAnimator.SetTrigger("ToIdle");

        // tampilkan panel konfirmasi
        if (panelKonfirmasi != null)
            panelKonfirmasi.SetActive(true);

        // mulai transisi awan & logo
        StartCoroutine(TransisiAwan());
    }

    public void BerhentiKarenaTabrakan()
    {
        hasTriggeredPanel = true;
        speed = 0;

        // hentikan suara
        if (backsoundGameSource != null)
            backsoundGameSource.Stop();

        if (butoAudioSource != null)
            butoAudioSource.Stop();

        // animasi buto ke idle
        if (butoAnimator != null)
            butoAnimator.SetTrigger("ToIdle");
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

            if (backsoundLogoSource != null)
            {
                backsoundLogoSource.Play();
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
