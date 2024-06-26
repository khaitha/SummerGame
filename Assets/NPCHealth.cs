using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCHealth : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider UI component
    public float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;
    private bool isDying = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDying) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDying) return;
        isDying = true;

        // Trigger the death animation
        animator.SetTrigger("DieTrigger");

        // Start coroutine to destroy the boss after the animation
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the death animation starts playing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("NPC Death")) // Replace with your death animation state name
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Wait for the length of the death animation
        yield return new WaitForSeconds(stateInfo.length);

        // Log death
        Debug.Log("NPC Died");

        // Destroy all children of the boss object recursively
        DestroyChildren(transform);

        // Destroy the boss GameObject
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
