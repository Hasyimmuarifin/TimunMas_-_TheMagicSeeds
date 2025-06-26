using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundScroller : MonoBehaviour
{
    [Header("Scrolling Settings")]
    public float speed = 2f;
    public float resetX = 60f;
    public float stopAtX = -20f;

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "kalah";
    [SerializeField] private int maxLoops = 5;
    [SerializeField] private bool isCounter = false; // hanya satu GameObject yang mengaktifkan ini!

    private static int loopCount = 0;
    private static bool hasLoadedScene = false;

    void Update()
    {
        if (hasLoadedScene) return;

        // Geser objek ke kiri
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= stopAtX)
        {
            // Reset posisi objek ke kanan
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);

            // Hanya GameObject "counter" yang hitung loop
            if (isCounter)
            {
                loopCount++;
                Debug.Log($"Loop ke-{loopCount}");

                if (loopCount >= maxLoops)
                {
                    hasLoadedScene = true;
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}
