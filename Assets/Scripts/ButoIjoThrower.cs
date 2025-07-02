using System.Collections;
using UnityEngine;

public class ButoIjoThrower : MonoBehaviour
{
    public GameObject[] projectilePrefabs;      // Array prefab proyektil acak
    public Transform throwPoint;                // Titik lemparan
    public float minThrowInterval = 2f;
    public float maxThrowInterval = 5f;

    private Animator animator;
    private bool isThrowing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ThrowLoop());
    }

    IEnumerator ThrowLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minThrowInterval, maxThrowInterval);
            yield return new WaitForSeconds(waitTime);

            // Aktifkan animasi lempar
            animator.SetTrigger("Throw");

            // Tunggu sedikit agar sinkron dengan animasi lempar
            yield return new WaitForSeconds(0.3f);

            // Aktifkan animasi lempar
            animator.SetTrigger("ToIdle");

            ThrowProjectile();
        }
    }

    void ThrowProjectile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        GameObject selectedProjectile = projectilePrefabs[index];

        Instantiate(selectedProjectile, throwPoint.position, throwPoint.rotation);
    }
}