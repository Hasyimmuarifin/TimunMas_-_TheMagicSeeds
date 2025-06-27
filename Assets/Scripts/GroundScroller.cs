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
    public GameObject logoImage;
    public float delayAntarAwan = 0.5f;
    public float delayLogo = 1f;
    public float delaySebelumLoad = 2f;

    private static int loopCount = 0;
    private static bool hasTriggeredPanel = false;

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

                    // 1. Ubah animasi buto ke Idle
                    if (butoAnimator != null)
                    {
                        butoAnimator.SetTrigger("ToIdle"); // pastikan transisi dibuat di Animator
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
        // 4. Munculkan awan satu per satu
        foreach (GameObject awan in awanImages)
        {
            if (awan != null)
            {
                awan.SetActive(true);
                yield return new WaitForSeconds(delayAntarAwan);
            }
        }

        // 5. Munculkan logo
        if (logoImage != null)
        {
            yield return new WaitForSeconds(delayLogo);
            logoImage.SetActive(true);
        }

        // 6. Tunggu sebelum load scene
        yield return new WaitForSeconds(delaySebelumLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Jika kamu ingin tetap ada tombol manual, kamu bisa pakai fungsi ini juga
    public void LanjutkanKeSceneBerikutnya()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
