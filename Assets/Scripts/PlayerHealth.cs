using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public AudioClip hitSFX;             // Menyimpan suara saat terkena damage 
    public AudioSource audioSource;      // Komponen AudioSource untuk memutar suara 
    public int maxHealth = 8;
    public int currentHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Jika audio dan sound effect tersedia, mainkan suara terkena hit 
        if (hitSFX != null && audioSource != null) 
        { 
            audioSource.PlayOneShot(hitSFX); // Mainkan suara sekali tanpa menghentikan audio lain 
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (playerMovement != null)
            {
                playerMovement.TriggerDeath(); // Panggil method khusus untuk efek kematian
            }
            else
            {
                Debug.LogWarning("PlayerMovement tidak ditemukan untuk TriggerDeath.");
            }
        }
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void GameOver()
    {
        Debug.Log("Timun Mas Kalah!");
        SceneManager.LoadScene("title");
    }
}