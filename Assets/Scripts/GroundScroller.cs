using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundScroller : MonoBehaviour
{
    public float speed = 2f;
    public float stopAtX = -50f;

    public static bool shouldStop = false;

    private static bool hasLoadedNextScene = false; // agar scene hanya dipanggil sekali

    void Update()
    {
        if (!shouldStop)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= stopAtX)
            {
                shouldStop = true;

                // Pindah scene hanya sekali
                if (!hasLoadedNextScene)
                {
                    hasLoadedNextScene = true;
                    LoadNextScene();
                }
            }
        }
    }

    void LoadNextScene()
    {
        // Ganti ini sesuai kebutuhan:
        // Bisa nama scene:
        SceneManager.LoadScene("SceneLevelBerikutnya");

        // Atau pakai build index:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
