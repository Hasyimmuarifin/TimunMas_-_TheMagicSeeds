using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;

    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Gunakan ini saat ingin pindah scene dan mencatatnya
    public void LoadSceneWithTracking(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Simpan scene saat ini sebelum pindah
        if (sceneHistory.Count == 0 || currentScene != sceneHistory.Peek())
        {
            sceneHistory.Push(currentScene);
        }

        SceneManager.LoadScene(sceneName);
    }

    // Untuk kembali ke scene sebelumnya
    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop(); // Ambil dan hapus dari stack
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("Tidak ada scene sebelumnya.");
        }
    }

    public void ClearHistory()
    {
        sceneHistory.Clear();
    }
}
