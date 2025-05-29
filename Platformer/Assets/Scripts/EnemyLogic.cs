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
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Death Effects")]
    public AudioClip deathClip;

    [Header("Attack Effects")]
    public AudioClip attackClip;


    //[Header("UI Refs")]


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
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
        if (gameObject.tag == "Patrol Enemy") 
        {
            animator.SetTrigger("Is Taking Hit");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        ////check !!!!!!!
        if (gameObject.tag == "Shooting Enemy")
        {
            animator.SetTrigger("Die");
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void OnAttack()
    {
        animator.SetTrigger("Is Attacking");
        PlayAttackSound();
    }


    public void Die()
    {
        animator.SetTrigger("Die");
        PlayDeathSound();
        Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Player2"))
        {
            //TakeDamage(player2.damageCapacity);
        }
        else if (collision.gameObject.CompareTag("Player1"))
        {
            //TakeDamage(player1.arrowDamageCapacity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
