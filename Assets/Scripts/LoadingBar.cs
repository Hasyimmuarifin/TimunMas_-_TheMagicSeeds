using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    public Image fillImage;
    public float loadDuration = 5f;
    private float timer;

    void Update()
    {
        if (timer < loadDuration)
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(timer / loadDuration);
        }
        else
        {
            SceneManager.LoadScene("play_awal"); // Ganti dengan nama scene tujuanmu
        }
    }
}
