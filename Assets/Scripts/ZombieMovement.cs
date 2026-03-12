using UnityEngine;

// Handles lightweight, physics-based wandering for zombie NPCs.
public class ZombieMovement : MonoBehaviour
{
    [Header("Wander Settings")]
    public float moveSpeed = 1f;
    public float turnInterval = 3f;

    private Rigidbody rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is used instead of Update for Rigidbody manipulation to ensure consistent physics steps
    void FixedUpdate()
    {
        // Continuously drive the zombie forward along its local Z-axis
        Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        timer += Time.fixedDeltaTime;

        // Periodically pick a new random rotation to simulate aimless wandering
        if (timer > turnInterval)
        {
            float randomTurn = Random.Range(-120f, 120f);
            Quaternion turn = Quaternion.Euler(0, randomTurn, 0);

            rb.MoveRotation(rb.rotation * turn);

            timer = 0;
        }
    }
}