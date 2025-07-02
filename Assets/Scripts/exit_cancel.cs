using UnityEngine;

public class exit_cancle : MonoBehaviour
{
    public AudioSource audioSource;     // Komponen untuk memutar suara
    public AudioClip clickSound;        // Suara klik

    public void OnCancelClick()
    {
        // Mainkan suara klik
        audioSource.PlayOneShot(clickSound);

        // Setelah suara selesai, balik ke scene sebelumnya
        Invoke(nameof(BackToPreviousScene), clickSound.length);
    }

    void BackToPreviousScene()
    {
        SceneTracker.Instance.GoBack();
    }
}
