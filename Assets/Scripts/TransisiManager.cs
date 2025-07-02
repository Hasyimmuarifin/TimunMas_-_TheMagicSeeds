using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransisiManager : MonoBehaviour
{
    [Header("Panel Transisi")]
    public GameObject panelTransisi;
    public float durasiTransisi = 1.5f;

    [Header("Transisi Keluar - Animasi")]
    public GameObject[] awanImages;
    public CanvasGroup[] awanCanvasGroups;
    public GameObject logoImage;
    public CanvasGroup logoCanvasGroup;
    [SerializeField] private string sceneToLoad = "kalah";

    [Header("Audio Settings")]
    public AudioSource backsoundLogoSource;
    public AudioSource backsoundGameSource;
    public AudioSource butoAudioSource;

    [Header("Delay dan Durasi")]
    public float delaySebelumAwan = 1f;
    public float delayAntarAwan = 0.4f;
    public float delayLogo = 0.5f;
    public float durasiFade = 1f;
    public float delaySebelumLoad = 1f;

    private bool sudahTransisiMasuk = false;

    void Start()
    {
        if (!sudahTransisiMasuk)
        {
            panelTransisi.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(TransisiMasukDenganAwan());
        }
    }

    IEnumerator TransisiMasukDenganAwan()
    {
        // Pastikan logo aktif dan fade-in (jika diperlukan)
        if (logoImage != null && logoCanvasGroup != null)
        {
            logoImage.SetActive(true);

            if (backsoundLogoSource != null)
                backsoundLogoSource.Play();

            // Pastikan logo terlihat
            logoCanvasGroup.alpha = 1f;

            // Tunggu sebentar sebelum fade out logo
            yield return new WaitForSecondsRealtime(delayLogo);

            // Fade out logo
            yield return StartCoroutine(FadeCanvasGroup(logoCanvasGroup, 1f, 0f, durasiFade));
            logoImage.SetActive(false);
        }

        // Fade out awan satu per satu
        yield return new WaitForSecondsRealtime(delaySebelumAwan);

        for (int i = 0; i < awanImages.Length; i++)
        {
            if (awanImages[i] != null && awanCanvasGroups[i] != null)
            {
                awanImages[i].SetActive(true); // pastikan terlihat
                awanCanvasGroups[i].alpha = 1f;

                yield return StartCoroutine(FadeCanvasGroup(awanCanvasGroups[i], 1f, 0f, durasiFade));
                awanImages[i].SetActive(false);

                yield return new WaitForSecondsRealtime(delayAntarAwan);
            }
        }

        // Setelah semua animasi selesai, mulai permainan
        yield return new WaitForSecondsRealtime(durasiTransisi);
        panelTransisi.SetActive(false);
        Time.timeScale = 1f;
        sudahTransisiMasuk = true;

        // Jika ada backsoundGame, play sekarang
        if (backsoundGameSource != null)
            backsoundGameSource.Play();
    }

    public void TransisiKeluar(string namaSceneBerikutnya)
    {
        StartCoroutine(TransisiKeluarCoroutine(namaSceneBerikutnya));
    }

    IEnumerator TransisiKeluarCoroutine(string sceneName)
    {
        panelTransisi.SetActive(true);
        Time.timeScale = 0f;

        // Logo keluar
        if (logoImage != null && logoCanvasGroup != null)
        {
            yield return new WaitForSecondsRealtime(delayLogo);
            yield return StartCoroutine(FadeCanvasGroup(logoCanvasGroup, 1f, 0f, durasiFade));
            logoImage.SetActive(false);
        }

        // Awan keluar satu per satu
        yield return new WaitForSecondsRealtime(delaySebelumAwan);

        for (int i = 0; i < awanImages.Length; i++)
        {
            if (awanImages[i] != null && awanCanvasGroups[i] != null)
            {
                yield return StartCoroutine(FadeCanvasGroup(awanCanvasGroups[i], 1f, 0f, durasiFade));
                awanImages[i].SetActive(false);
                yield return new WaitForSecondsRealtime(delayAntarAwan);
            }
        }

        yield return new WaitForSecondsRealtime(delaySebelumLoad);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator TransisiAwan()
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
                backsoundLogoSource.Play();

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
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
    }
}