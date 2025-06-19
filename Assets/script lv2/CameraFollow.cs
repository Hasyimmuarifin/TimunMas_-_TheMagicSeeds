using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Target yang diikuti (biasanya Player)
    public Vector3 offset;        // Jarak kamera dari target
    public float smoothSpeed = 0.125f;  // Kehalusan gerakan kamera

    void LateUpdate()
    {
        if (target != null)
        {
            // Posisi kamera mengikuti target (misal hanya X saja)
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, offset.y, offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
