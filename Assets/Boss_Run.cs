using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 4f;
    public float followRange = 8f; // New variable for follow range

    Transform player;
    Rigidbody2D rb;
    Boss boss;
    int attacksCount = 0;
    bool isAttacking = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        isAttacking = false; // Ensure isAttacking is reset
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isAttacking)
            return;

        boss.LookAtPlayer();

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        // If the player is within attack range, trigger an attack
        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            attacksCount++;
            Debug.Log("Attack triggered. Count: " + attacksCount);

            // Start the coroutine to wait for the attack animation to finish
            animator.GetComponent<MonoBehaviour>().StartCoroutine(WaitForAttackToFinish(animator));
        }
        // If the player is within follow range but not in attack range, start following the player
        else if (distanceToPlayer <= followRange)
        {
            Vector2 target = new Vector2(player.position.x + 2, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    IEnumerator WaitForAttackToFinish(Animator animator)
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    // Draw gizmos for visualizing the follow range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rb.position, followRange);
    }
}
