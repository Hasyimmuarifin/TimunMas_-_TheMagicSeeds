using UnityEngine;

public class home_button : MonoBehaviour
{
    public AudioSource audioSource;     // AudioSource untuk memutar suara
    public AudioClip clickSound;        // Suara klik

    private string _nextScene;

    public void OnButtonClick(string sceneName)
    {
        audioSource.PlayOneShot(clickSound);
        _nextScene = sceneName;

        // Tunggu sampai suara selesai sebelum load scene
        Invoke(nameof(LoadSceneWithTracking), clickSound.length);
    }

    void LoadSceneWithTracking()
    {
        SceneTracker.Instance.LoadSceneWithTracking(_nextScene);
    }
}
