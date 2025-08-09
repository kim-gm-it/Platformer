using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float swordOffset = 4.7f;
    public float retreatDistanceBuffer = 0.5f;

    Rigidbody2D rb;
    Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();

        if (rb == null) Debug.LogError("Rigidbody2D not found on Boss!");
        if (boss == null) Debug.LogError("Boss script not found on Boss!");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform currentTarget = boss.GetCurrentTarget();
        if (currentTarget == null || rb == null || boss == null)
        {
            // If there's no target or components are missing, stop the boss.
            rb.linearVelocity = Vector2.zero;
            return;
        }

        boss.LookAtPlayer(currentTarget);

        float distanceToPlayer = Vector2.Distance(currentTarget.position, rb.position);
        float targetDistanceForAttack = swordOffset;

        Vector2 moveDirection = Vector2.zero;

        if (distanceToPlayer < targetDistanceForAttack - retreatDistanceBuffer)
        {
            // Retreat from the player
            Vector2 retreatDirection = ((Vector2)rb.position - (Vector2)currentTarget.position).normalized;
            moveDirection = retreatDirection;
        }
        else if (distanceToPlayer <= targetDistanceForAttack + retreatDistanceBuffer && distanceToPlayer >= targetDistanceForAttack - retreatDistanceBuffer)
        {
            // Stop and attack
            moveDirection = Vector2.zero;
            animator.SetTrigger("Attack");
        }
        else
        {
            // Chase the player
            Vector2 chaseDirection = ((Vector2)currentTarget.position - (Vector2)rb.position).normalized;
            moveDirection = chaseDirection;
        }

        // Apply velocity to the Rigidbody for physics-based movement
        rb.linearVelocity = moveDirection * speed;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        // Stop the boss when exiting the state
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}