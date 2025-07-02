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
            StartCoroutine(TransisiMasukSelesai());
        }
    }

    IEnumerator TransisiMasukSelesai()
    {
        yield return new WaitForSecondsRealtime(durasiTransisi);
        panelTransisi.SetActive(false);
        Time.timeScale = 1f;
        sudahTransisiMasuk = true;
    }

    public void TransisiKeluar(string namaSceneBerikutnya)
    {
        StartCoroutine(TransisiKeluarCoroutine(namaSceneBerikutnya));
    }

    IEnumerator TransisiKeluarCoroutine(string sceneName)
    {
        panelTransisi.SetActive(true);
        Time.timeScale = 0f;
        if (logoImage != null && logoCanvasGroup != null)
        {
            yield return new WaitForSecondsRealtime(delayLogo);
            yield return StartCoroutine(FadeCanvasGroup(logoCanvasGroup, 1f, 0f, durasiFade));
            logoImage.SetActive(false);
        }
        // Kemudian awan keluar satu per satu
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