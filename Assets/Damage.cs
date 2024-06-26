using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    public float damageOnContact = 10f;
    private PlayerHealth playerHealth;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Deal damage to the player on contact
                playerHealth.TakeDamage(damageOnContact);
            }
        }
    }
}
