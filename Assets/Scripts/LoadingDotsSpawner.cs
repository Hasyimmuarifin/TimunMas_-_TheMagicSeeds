using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingDotsSpawner : MonoBehaviour
{
    public GameObject dotTemplate;         // prefab titik (Image) yang akan diklon
    public RectTransform dotParent;        // tempat menaruh titik (DotContainer)
    public int maxDots = 3;                // jumlah titik maksimal (misalnya 3)
    public float switchRate = 0.5f;        // jeda antar munculnya titik
    public float totalDuration = 5f;       // durasi total loading
    public string nextScene = "play_awal";  // nama scene tujuan

    private List<GameObject> activeDots = new List<GameObject>();
    private float timer = 0f;
    private int dotCount = 0;

    void Start()
    {
        Invoke("LoadNextScene", totalDuration); // jadwalkan pindah scene
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= switchRate)
        {
            timer = 0f;
            UpdateDots();
        }
    }

    void UpdateDots()
    {
        // Hapus semua titik sebelumnya
        foreach (var dot in activeDots)
        {
            Destroy(dot);
        }
        activeDots.Clear();

        // Tambahkan jumlah titik baru (bertambah sampai maxDots lalu ulang)
        dotCount = (dotCount + 1) % (maxDots + 1);
        for (int i = 0; i < dotCount; i++)
        {
            GameObject newDot = Instantiate(dotTemplate, dotParent);
            newDot.SetActive(true);

            // Geser posisi titik secara horizontal
            RectTransform rt = newDot.GetComponent<RectTransform>();
            rt.anchoredPosition += new Vector2(i * 10.5f, 0);

            activeDots.Add(newDot);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
