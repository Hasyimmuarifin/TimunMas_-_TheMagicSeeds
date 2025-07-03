using UnityEngine;
using System.Collections;

public class PersistentTransisi : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public GameObject[] awanImages;
    public CanvasGroup[] awanCanvasGroups;

    public float delayBeforeFadeOut = 2f;
    public float durasiFadeOut = 1f;
    public float delayAntarAwanFadeOut = 0.5f;

    void Start()
    {
        // Jaga agar object ini tidak dihancurkan saat pindah scene
        DontDestroyOnLoad(gameObject);

        // Mulai coroutine untuk fade out di scene baru
        StartCoroutine(FadeOutSequence());
    }

    IEnumerator FadeOutSequence()
    {
        yield return new WaitForSeconds(delayBeforeFadeOut);

        // Fade out logo
        if (logoCanvasGroup != null)
            yield return StartCoroutine(FadeCanvasGroup(logoCanvasGroup, 1f, 0f, durasiFadeOut));

        // Fade out awan satu persatu
        for (int i = 0; i < awanCanvasGroups.Length; i++)
        {
            if (awanCanvasGroups[i] != null)
            {
                yield return StartCoroutine(FadeCanvasGroup(awanCanvasGroups[i], 1f, 0f, durasiFadeOut));
                yield return new WaitForSeconds(delayAntarAwanFadeOut);
            }
        }

        // Destroy setelah selesai transisi
        Destroy(gameObject);
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
}
