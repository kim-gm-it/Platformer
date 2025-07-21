using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 1;

    public Vector3 attackOffset; 
    
    public float attackRange = 1f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 currentAttackOffset = attackOffset;
        currentAttackOffset.x *= transform.localScale.x; 

        Vector3 pos = transform.position + currentAttackOffset;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, attackRange, attackMask);

        foreach (Collider2D col in hitColliders)
        {
            var player1 = col.GetComponent<player1_level3>();
            if (player1 != null)
            {
                player1.TakeDamage();
            }

            var player2 = col.GetComponent<player2_level3>();
            if (player2 != null)
            {
                player2.TakeDamage();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 currentAttackOffset = attackOffset;
        
        currentAttackOffset.x *= transform.localScale.x; 

        Vector3 pos = transform.position + currentAttackOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}