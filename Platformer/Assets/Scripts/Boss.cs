using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Boss Targets")]
    public Transform[] players;
    public int currentPlayerIndex = 0;
    public float switchTimer = 0f;
    public float switchTargetTime = 5f;

    [Header("Boss Stats")]
    public bool isFlipped = false;
    public int maxHealth = 100;
    public int currentHealth;
    private bool isDead = false;

    [Header("References")]
    private Animator animator;
    // public EnemyHealthBar healthBar;

    [Header("Spawn Variables")]
    public float spawnInterval = 10f;
    public SpawnManager spawnManager;
    private float spawnTimer;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip breathingSFX;
    public AudioClip attackSFX;
    public AudioClip hurtSFX;
    public AudioClip deathSFX;
    [Header("UI Panels")]
    public GameObject winPanel;
    private GameStateManager gameStateManagerB;
    public Slider slider;


    void Start()
    {
        spawnTimer = spawnInterval;
        gameStateManagerB = FindFirstObjectByType<GameStateManager>();

        players = new Transform[]
        {
            GameObject.FindGameObjectWithTag("Player1")?.transform,
            GameObject.FindGameObjectWithTag("Player2")?.transform
        };

        switchTimer = switchTargetTime;

        if (currentHealth <= 0)
            currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        UpdateHealthBar(currentHealth, maxHealth);

        if (breathingSFX != null)
        {
            audioSource.clip = breathingSFX;
            audioSource.loop = true;
            audioSource.Play();
        }
    }


    private void Update()
    {
        if (isDead) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnManager.SpawnEenmy();
            spawnTimer = spawnInterval;
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");

        if (deathSFX != null) audioSource.PlayOneShot(deathSFX);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        StartCoroutine(HandleVictorySequence(0.6f));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player2") || collision.gameObject.CompareTag("Player Arrow"))
        {
            TakeDamage(4);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Player2") || collision.CompareTag("Player Arrow"))
        {
            TakeDamage(4);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (hurtSFX != null) audioSource.PlayOneShot(hurtSFX);

        UpdateHealthBar(currentHealth, maxHealth);

        ////////////////////////////////
        Debug.Log("Current Health : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void PlayAttackSound()
    {
        if (attackSFX != null) audioSource.PlayOneShot(attackSFX);
    }

    public Transform GetCurrentTarget()
    {
        if (players.Length == 0) return null;

        int startIndex = currentPlayerIndex;
        do
        {
            if (players[currentPlayerIndex] != null)
            {
                break;
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            if (currentPlayerIndex == startIndex)
                return null;

        } while (players[currentPlayerIndex] == null);

        switchTimer -= Time.deltaTime;
        if (switchTimer <= 0f)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            int attempts = 0;
            while (players[currentPlayerIndex] == null && attempts < players.Length)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                attempts++;
            }

            switchTimer = switchTargetTime;
        }

        return players[currentPlayerIndex];
    }


    public void LookAtPlayer(Transform target)
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;

        if (transform.position.x > target.position.x && isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = false;
        }
        else if (transform.position.x < target.position.x && !isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = true;
        }
    }
    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator HandleVictorySequence(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameStateManagerB != null)
        {
            gameStateManagerB.ShowYouWin();
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    public void UpdateHealthBar(float current, float max)
    {
        slider.value = current / max;
    }
}