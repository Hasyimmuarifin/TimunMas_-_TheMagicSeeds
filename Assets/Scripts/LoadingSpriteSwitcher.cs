using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSpriteSwitcher : MonoBehaviour
{
    public Sprite[] loadingSprites;          // Array gambar Loading., .., ...
    public float switchRate = 0.5f;          // Waktu antar sprite
    public string nextScene = "play_awal";    // Nama scene tujuan
    public float totalDuration = 5f;         // Lama loading sebelum pindah

    private Image loadingImage;
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        loadingImage = GetComponent<Image>();
        loadingImage.sprite = loadingSprites[0];

        // Setelah totalDuration detik, pindah scene
        Invoke("GoToNextScene", totalDuration);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= switchRate)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % loadingSprites.Length;
            loadingImage.sprite = loadingSprites[currentIndex];
        }
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
