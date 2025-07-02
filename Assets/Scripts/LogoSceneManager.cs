using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneManager : MonoBehaviour
{
    public float delay = 5f; // durasi animasi
    public string nextScene = "loading"; // ganti dengan nama scene selanjutnya

    void Start()
    {
        Invoke("LoadNextScene", delay);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
