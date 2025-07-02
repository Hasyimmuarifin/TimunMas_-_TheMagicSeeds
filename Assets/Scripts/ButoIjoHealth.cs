using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButoIjoHealth : MonoBehaviour
{
    public int currentHP = 100;
    public int maxHP = 100;

    public int layerIndex; // 7-14 untuk 4 scene kejar - 4 scene lempar
    public string nextSceneName;
    [Header("UI")]
    public Slider healthSlider;

    void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // jangan sampai minus
        if (healthSlider != null)
        {
            healthSlider.value = currentHP;
        }
        if (currentHP <= 0)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (layerIndex < 14)
            SceneManager.LoadScene(nextSceneName); // contoh: "Level2"
        else
            Debug.Log("Buto Ijo Kalah Total!");
    }
}