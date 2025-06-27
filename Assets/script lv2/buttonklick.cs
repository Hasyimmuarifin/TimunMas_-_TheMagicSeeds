using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonclick : MonoBehaviour
{
    public string namaSceneTujuan;

    public void PindahKeScene()
    {
        SceneManager.LoadScene(namaSceneTujuan);
    }
}
