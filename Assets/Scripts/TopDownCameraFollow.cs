using UnityEngine;

// Snaps the camera to the player's position while maintaining a top-down perspective.
public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Tracking Settings")]
    public Transform target;

    // Default offset angled for a top-down portrait view
    public Vector3 offset = new Vector3(0, 18, -10);

    // LateUpdate is used to ensure the camera moves AFTER the car has finished its movement frame, preventing jitter.
    void LateUpdate()
    {
        // Safety check to prevent null reference errors
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}