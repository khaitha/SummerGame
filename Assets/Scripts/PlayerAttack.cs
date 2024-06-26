using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float blockDuration = 1f; // Duration for how long the block lasts
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private Collider2D bossInRange;
    private bool isBlocking = false;
    private float blockTimer = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && cooldownTimer > attackCooldown)
            StartCoroutine(Attack());

        if (Input.GetKeyDown(KeyCode.K))
            StartCoroutine(Block());

        if (isBlocking)
        {
            blockTimer += Time.deltaTime;
            if (blockTimer >= blockDuration)
            {
                isBlocking = false;
                blockTimer = 0;
                anim.ResetTrigger("Block");
                Debug.Log("Player stopped blocking due to time out.");
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("Punch");
        cooldownTimer = 0;

        // Delay for the attack animation
        yield return new WaitForSeconds(0.1f);

        // Deal damage to the boss if in range
        if (bossInRange != null && bossInRange.CompareTag("Boss"))
        {
            Debug.Log("Attacking boss!");
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
        else
        {
            Debug.Log("No boss in range.");
        }
    }

    private IEnumerator Block()
    {
        isBlocking = true;
        anim.SetTrigger("Block");
        blockTimer = 0;
        Debug.Log("Player is blocking");

        // Delay for the block duration
        yield return new WaitForSeconds(blockDuration);

        isBlocking = false;
        anim.ResetTrigger("Block");
        Debug.Log("Player stopped blocking");
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            bossInRange = other;
            Debug.Log("Boss entered attack range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            bossInRange = null;
            Debug.Log("Boss exited attack range.");
        }
    }

    // Draw a gizmo to visualize the attack range in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
