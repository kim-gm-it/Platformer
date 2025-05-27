using UnityEngine;

public class AttackPlayer1 : MonoBehaviour
{
    [SerializeField] int lives = 5;
    [SerializeField] private Animator animator;
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            lives -= 1;
            animator.SetBool("Hit", true);
            if (lives <= 0)
            {
                animator.SetBool("Death", true);
            }
        }
        animator.SetBool("Hit", false);
        animator.SetBool("Death", false);
    }
}
