using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy Refs")]
    private Renderer renderer;
    private Collider2D collider;
    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    Player2Controler player2;
    Player1Controller player1;

    [Header("Health Setting")]
    public int maxHealth = 4;
    public int currentHealth;
    EnemyHealthBar healthBar;
    
    [Header("Death Effects")]
    public AudioClip deathClip;


    [Header("Attack Effects")]
    public AudioClip attackClip;

    [Header("Player's Hit damage")]
    public int meleeDamage = 1;
    public int arrowDamage = 2;



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
    
        animator.SetTrigger("Is Taking Hit");
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathSequence());
        }
         
    }

    public void OnAttack()
    {
        animator.SetTrigger("Is Attacking");
        PlayAttackSound();
    }

    private IEnumerator DeathSequence()
    {
        Die();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    public void Die()
    {
        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Die");
        PlayDeathSound();
        renderer.enabled = false;
        collider.enabled = false;
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

        if (collision.gameObject.CompareTag("Player Arrow"))
        {
            TakeDamage(arrowDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player2"))
        {

            TakeDamage(meleeDamage);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }

}
