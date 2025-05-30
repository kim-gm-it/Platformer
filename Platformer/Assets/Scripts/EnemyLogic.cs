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
    public int maxLives = 3;
    public int currentHealth;
    public int currentLives;

    [Header("Death Effects")]
    public AudioClip deathClip;
    public float spawnTime = 1.5f;
    public float spawnTimer = 0;

    [Header("Attack Effects")]
    public AudioClip attackClip;

    [Header("Player's Hit damage")]
    public int meleeDamage = 1;
    public int arrowDamage = 2;


    [Header("UI Refs")]
    public Image[] lives;
    public Image[] healthBar;
   



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnTime;

        currentHealth = maxHealth;
        currentLives = maxLives;
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
        updateHealthBar();
       
        if (currentHealth <= 0)
        {
            for(int i=0; i < lives.Length ; i++)
            {
                if(i < currentLives)
                {
                    lives[i].enabled = true ;
                }
                else
                {
                    lives[i].enabled=false ;
                }
            }

            Die();
            currentLives--;
            if(currentLives <= 0)
            {
                Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Player2"))
        {
            TakeDamage(meleeDamage);
        }
        else if (collision.gameObject.CompareTag("Player1"))
        {
            TakeDamage(meleeDamage);
        }
        else if(collision.gameObject.CompareTag("Player Arrow"))
        {
            TakeDamage(arrowDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0 && currentLives > 0 && !renderer.enabled)
        {
            currentHealth = maxHealth;
            updateHealthBar();
            collider.enabled = true;
            renderer.enabled = true;
            spawnTimer = spawnTime;
            
        }

    }

    public void updateHealthBar()
    {
        for(int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].enabled = i < currentHealth;
        }
    }
}
