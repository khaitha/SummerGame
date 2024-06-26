using UnityEngine;

public class FireFlame : MonoBehaviour
{
    public float activeTime = 2f; // Time in seconds the flame thrower remains active
    [SerializeField] private float attackDamage = 50.0f;
    private float timer; // Timer to keep track of active time
    private Collider2D bossInRange;

    void Start()
    {
        timer = activeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject); // Destroy the flame thrower after activeTime seconds
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the other collider is a boss or enemy
        if (other.CompareTag("Boss"))
        {
            bossInRange = other; // Set bossInRange to the collider of the boss


            // Apply damage to the boss health component
            BossHealth bossHealth = bossInRange.GetComponent<BossHealth>();
            NPCHealth npcHealth = bossInRange.GetComponent<NPCHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(attackDamage);
            }
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Clear bossInRange when the boss exits the trigger
        if (other == bossInRange)
        {
            bossInRange = null;
        }
    }
}
