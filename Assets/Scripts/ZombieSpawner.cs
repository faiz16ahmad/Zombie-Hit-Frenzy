using UnityEngine;
using System.Collections;

// Manages the arena's zombie population, ensuring a constant supply of targets for the player.
public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject zombiePrefab;
    public float spawnRadius = 20f;

    void Start()
    {
        // Populate the arena immediately. 
        // 15 instances safely exceeds the minimum test requirement of 10 active NPCs.
        for (int i = 0; i < 15; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        // Calculate a random spawn point within the defined arena radius.
        // A slight Y-offset (0.1f) is applied to prevent the collider from clipping through the floor.
        Vector3 pos = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0.1f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        Instantiate(zombiePrefab, pos, Quaternion.identity);
    }

    // Coroutine triggered by the ZombieRagdoll script when a zombie is hit
    public IEnumerator SpawnReplacementZombie()
    {
        // Enforce the required 2-to-3-second delay before replenishing the target
        float delay = Random.Range(2f, 3f);

        yield return new WaitForSeconds(delay);

        SpawnZombie();
    }
}