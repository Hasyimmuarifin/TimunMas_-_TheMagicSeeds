using UnityEngine;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    public AudioSource audioSource;     // AudioSource untuk memutar suara
    public AudioClip clickSound;        // Suara klik

    public void OnButtonClick(string sceneName)
    {
        audioSource.PlayOneShot(clickSound);
        Invoke(nameof(LoadScene), clickSound.length);
        _nextScene = sceneName;
    }

    private string _nextScene;

    void LoadScene()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
