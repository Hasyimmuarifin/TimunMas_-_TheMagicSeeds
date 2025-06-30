using UnityEngine;
using UnityEngine.SceneManagement; // Untuk Scene Management jika ingin meload scene lain

public class EscapeToQuit : MonoBehaviour
{
    public GameObject escapeUI; // Referensi ke Canvas atau Panel konfirmasi keluar
    private bool isPopupShown = false;

    void Start()
    {
        // Pastikan UI konfirmasi keluar dimulai dalam keadaan tersembunyi
        escapeUI.SetActive(false); // Pastikan escapeUI dimulai dalam keadaan tersembunyi
    }

    void Update()
    {
        // Saat tombol Escape ditekan, dan popup belum muncul
        if (Input.GetKeyDown(KeyCode.Escape) && !isPopupShown)
        {
            // Tampilkan UI popup tanpa mengganti scene
            escapeUI.SetActive(true);  // Menampilkan Canvas PauseUI
            isPopupShown = true;       // Menandakan bahwa popup telah muncul
            Time.timeScale = 0f;       // Pause game saat popup muncul
        }
    }

    // Fungsi untuk melanjutkan game
    public void ResumeGame()
    {
        // Tutup popup dan lanjutkan game
        escapeUI.SetActive(false); // Menyembunyikan Canvas PauseUI
        Time.timeScale = 1f;       // Resume game
        isPopupShown = false;      // Menandakan bahwa popup tidak aktif lagi
    }

    // Fungsi untuk keluar dari game (atau pindah ke scene lain)
    public void QuitGame()
    {
        // Pindah ke scene tertentu
        SceneManager.LoadScene("title"); // Ganti "MainMenu" dengan nama scene yang kamu tuju

        // Jika ingin keluar dari aplikasi
        // Application.Quit(); // Ini hanya akan berfungsi di build, bukan di editor
    }

    // Fungsi untuk mulai ulang game atau pindah ke scene lain (bisa diubah jika perlu)
    public void StartGame()
    {
        // Contoh jika ingin reload scene yang sama (restart level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload scene yang sama
        Time.timeScale = 1f; // Pastikan waktu permainan aktif
    }
}