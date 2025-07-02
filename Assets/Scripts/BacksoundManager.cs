using UnityEngine;
using UnityEngine.SceneManagement;

public class BacksoundManager : MonoBehaviour
{
    public static BacksoundManager instance;
    private AudioSource audioSource;

    [Header("Daftar Scene yang Diizinkan")]
    public string[] allowedScenes;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Hapus duplikat
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Biar tetap hidup di antar scene

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("BacksoundManager: AudioSource tidak ditemukan!");
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name.ToLowerInvariant();
        bool diizinkan = false;

        foreach (string namaScene in allowedScenes)
        {
            if (currentScene == namaScene.ToLowerInvariant())
            {
                diizinkan = true;
                break;
            }
        }

        Debug.Log($"[BacksoundManager] Scene saat ini: {scene.name}, Diizinkan: {diizinkan}");

        if (diizinkan)
        {
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("[BacksoundManager] Memutar backsound.");
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("[BacksoundManager] Backsound dihentikan karena scene tidak diizinkan.");
            }
        }
    }

    public void GantiLagu(AudioClip clip, bool play = true)
    {
        if (audioSource == null) return;

        audioSource.clip = clip;
        if (play)
        {
            audioSource.Play();
            Debug.Log("[BacksoundManager] Lagu diganti dan diputar.");
        }
    }

    public void SetVolume(float vol)
    {
        if (audioSource == null) return;
        audioSource.volume = vol;
        Debug.Log($"[BacksoundManager] Volume disetel ke: {vol}");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
