using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLogicLevel3 : MonoBehaviour
{
    [Header("Enemy Refs")]
    private Renderer renderer;
    private Collider2D collider;
    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isDead = false;
    Player2Controler player2;
    Player1Controller player1;

    [Header("Health Setting")]
    public int maxHealth = 2;
    public int currentHealth;
    EnemyHealthBar healthBar;

    [Header("Death Effects")]
    public AudioClip deathClip;


    [Header("Attack Effects")]
    public AudioClip attackClip;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        renderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        player2 = GameObject.Find("Player2").GetComponent<Player2Controler>();
        player1 = GameObject.Find("Player1").GetComponent<Player1Controller>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        Debug.Log("Enemy is taking damage");
        animator.SetBool("IsRunning", false);
        animator.SetTrigger("IsTakingHit");
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathSequence());
        }

    }

    public void OnAttack()
    {
        if (isDead) return;
        Debug.Log("Enemy is attacking");
        animator.SetBool("IsRunning", false);
        animator.SetTrigger("IsAttacking");
        PlayAttackSound();
    }

    private IEnumerator DeathSequence()
    {
        Die();
        healthBar.OnDestroy();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    public void Die()
    {
        isDead = true;
        animator.SetBool("IsRunning", false);
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Die");
        PlayDeathSound();
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClip);
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        DamageDealer dealer = collision.GetComponent<DamageDealer>();
        if (dealer != null)
        {

            TakeDamage(dealer.GetDamage());
            Debug.Log("Hit Enemy with " + dealer.GetDamage() + " damage ");
          
        }
    }

}
