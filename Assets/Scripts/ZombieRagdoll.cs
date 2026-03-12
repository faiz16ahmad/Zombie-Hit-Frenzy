using UnityEngine;

// Manages the transition from an animated state to a physics-driven ragdoll upon impact.
public class ZombieRagdoll : MonoBehaviour
{
    private Rigidbody[] bodies;
    private Collider[] boneColliders;

    private Animator anim;
    private ZombieMovement movement;
    private Rigidbody rootBody;
    private CapsuleCollider rootCollider;

    private Rigidbody pelvis;

    [Header("Physics Settings")]
    public float impactForce = 80f;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<ZombieMovement>();
        rootBody = GetComponent<Rigidbody>();
        rootCollider = GetComponent<CapsuleCollider>();

        bodies = GetComponentsInChildren<Rigidbody>();
        boneColliders = GetComponentsInChildren<Collider>();

        // Lock bone physics and colliders initially so the Animator can drive the mesh without interference
        foreach (Rigidbody rb in bodies)
        {
            if (rb != rootBody)
                rb.isKinematic = true;

            if (rb.name.ToLower().Contains("pelvis"))
                pelvis = rb;
        }

        foreach (Collider c in boneColliders)
        {
            if (c != rootCollider)
                c.enabled = false;
        }
    }

    public void ActivateRagdoll(Vector3 impactDirection)
    {
        // Disable AI movement and the Animator
        if (movement != null)
            movement.enabled = false;

        if (anim != null)
            anim.enabled = false;

        // Freeze the root body so the individual child bones can take over
        rootBody.isKinematic = true;

        // Unleash the ragdoll physics
        foreach (Rigidbody rb in bodies)
        {
            if (rb != rootBody)
                rb.isKinematic = false;
        }

        foreach (Collider c in boneColliders)
        {
            if (c != rootCollider)
                c.enabled = true;
        }

        // Apply directional momentum to the core mass (pelvis) for a realistic collapse
        if (pelvis != null)
        {
            pelvis.AddForce(impactDirection * impactForce, ForceMode.Impulse);

            // Add rotational torque for a more chaotic, dynamic impact reaction
            pelvis.AddTorque(Random.insideUnitSphere * impactForce * 0.5f, ForceMode.Impulse);
        }

        // Notify spawner to replenish the arena
        ZombieSpawner spawner = FindAnyObjectByType<ZombieSpawner>();
        if (spawner != null)
        {
            spawner.StartCoroutine(spawner.SpawnReplacementZombie());
        }
    }
}