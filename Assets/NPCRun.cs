using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRun : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 1f;
    public float followRange = 30f;
    Transform player;
    Rigidbody2D rb;
    NPC npc;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        npc = animator.GetComponent<NPC>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }

        if (npc == null)
        {
            Debug.LogError("NPC component not found!");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = Vector2.Distance(player.position, rb.position);
        if (distanceToPlayer <= followRange)
        {
            animator.SetBool("inRange", true);
            // Follow the player
            npc.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (distanceToPlayer <= attackRange)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.SetBool("inRange", false);
        rb.velocity = Vector2.zero;
        Debug.Log("NPC is not in range anymore.");
    }
}
