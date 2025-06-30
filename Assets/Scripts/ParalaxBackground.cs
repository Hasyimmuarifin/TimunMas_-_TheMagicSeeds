using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxFactor = 0.1f;

    private Transform cam;
    private Vector3 previousCamPosition;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPosition = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - previousCamPosition;
        transform.position += delta * parallaxFactor;
        previousCamPosition = cam.position;
    }
}