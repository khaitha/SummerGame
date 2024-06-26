using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI component
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator animator;

    private bool isDying = false;
    private PlayerAttack playerAttack; // Reference to PlayerAttack script

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        animator = GetComponent<Animator>(); // Initialize the animator reference
        playerAttack = GetComponent<PlayerAttack>(); // Initialize the playerAttack reference
    }

    public void TakeDamage(float damage)
    {
        if (isDying) return;

        // Check if the player is blocking
        if (playerAttack != null && playerAttack.IsBlocking())
        {
            Debug.Log("Blocking");
            damage *= 0.1f; // Reduce damage by 90%
            
        }

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (isDying) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Die()
    {
        if (isDying) return;
        isDying = true;

        // Trigger the death animation
        animator.SetTrigger("DieTrigger");

        // Start coroutine to destroy the player after the animation
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the death animation starts playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("DeathPlayer")) // Replace with your death animation state name
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Wait for the length of the death animation
        yield return new WaitForSeconds(stateInfo.length);

        // Log death
        Debug.Log("Player Died");

        // Destroy all children of the player object recursively
        DestroyChildren(transform);

        // Destroy the player GameObject
        Destroy(gameObject);
    }

    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Recursively destroy children of the current child
            DestroyChildren(child);

            // Destroy the current child GameObject
            Destroy(child.gameObject);
        }
    }
}
