using UnityEngine;

// Detects collisions with the player car and triggers the transition to an active ragdoll state.
public class ZombieHit : MonoBehaviour
{
    private ZombieRagdoll ragdoll;
    private CapsuleCollider col;

    private bool isDead = false;

    void Start()
    {
        ragdoll = GetComponent<ZombieRagdoll>();
        col = GetComponent<CapsuleCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Prevent multiple simultaneous hits from scoring twice
        if (isDead) return;

        // Check if the colliding object is the player (handles nested colliders via root)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            isDead = true;

            // Notify GameManager to increment the score
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }

            // Calculate impact direction based on the car's forward momentum
            Vector3 impactDirection = other.transform.forward;

            // Add a slight upward vector for better visual "pop" when the ragdoll activates
            impactDirection.y += 0.5f;
            impactDirection = impactDirection.normalized;

            // Disable the main upright collider so the car can drive over the body
            if (col != null)
                col.enabled = false;

            // Activate the physics chains and pass the collision momentum
            if (ragdoll != null)
                ragdoll.ActivateRagdoll(impactDirection);

            // Disable this specific script to save performance now that the zombie is defeated
            this.enabled = false;
        }
    }
}