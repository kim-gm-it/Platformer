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
            return;
        }

        boss.LookAtPlayer(currentTarget); 

        float distanceToPlayer = Vector2.Distance(currentTarget.position, rb.position);

        float targetDistanceForAttack = swordOffset; 

        if (distanceToPlayer < targetDistanceForAttack - retreatDistanceBuffer)
        {
            Vector2 retreatDirection = ((Vector2)rb.position - (Vector2)currentTarget.position).normalized;
            Vector2 retreatTarget = (Vector2)currentTarget.position + retreatDirection * (targetDistanceForAttack + retreatDistanceBuffer);
            
            Vector2 newPos = Vector2.MoveTowards(rb.position, retreatTarget, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else if (distanceToPlayer <= targetDistanceForAttack + retreatDistanceBuffer && distanceToPlayer >= targetDistanceForAttack - retreatDistanceBuffer)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            Vector2 moveTarget;
            if (boss.isFlipped) 
            {
                moveTarget = new Vector2(currentTarget.position.x + targetDistanceForAttack, currentTarget.position.y);
            }
            else 
            {
                moveTarget = new Vector2(currentTarget.position.x - targetDistanceForAttack, currentTarget.position.y);
            }

            Vector2 newPos = Vector2.MoveTowards(rb.position, moveTarget, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}