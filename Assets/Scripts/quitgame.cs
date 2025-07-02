using UnityEngine;

public class quitgame : MonoBehaviour
{
    public AudioSource audioSource;     // Komponen AudioSource
    public AudioClip clickSound;        // Suara klik tombol

    public void OnCancelClick()
    {
        // Mainkan suara klik
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            Invoke(nameof(QuitGame), clickSound.length);
        }
        else
        {
            QuitGame(); // Langsung keluar jika tidak ada audio
        }
    }

    void QuitGame()
    {
        Debug.Log("Keluar dari game..."); // Hanya terlihat di editor
        Application.Quit();
    }
}
