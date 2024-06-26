using UnityEngine;

public class NPCDeathAdjustment : StateMachineBehaviour
{
    public Vector2 deathPositionOffset; // Offset to adjust the death position

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.MovePosition(rb.position + deathPositionOffset);
        }
    }
}
