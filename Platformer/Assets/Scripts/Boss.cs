using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform[] players;
    public int currentPlayerIndex = 0;
    public float switchTimer = 0f;
    public float switchTargetTime = 10f;

    public bool isFlipped = false;
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private bool isDead = false;
    public EnemyHealthBar healthBar;

    [Header("spawn variables")]
    public float spawnInterval = 10f;
    public SpawnManager spawnManager;
    private float spawnTimer;
    

    void Start()
    {
        //initialize spawn timer
        spawnTimer = spawnInterval;


        players = new Transform[]
        {
            GameObject.FindGameObjectWithTag("Player1")?.transform,
            GameObject.FindGameObjectWithTag("Player2")?.transform
        };
        switchTimer = switchTargetTime;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Update()
    {
        Debug.Log(currentHealth);

        if(isDead) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnManager.SpawnEenmy();
            spawnTimer = spawnInterval;
        }
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Player2") || collision.gameObject.CompareTag("Player Arrow"))
        {
            int damage = 10; 
            TakeDamage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("Player2") || collision.CompareTag("Player Arrow"))
        {
            int damage = 10; 
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public Transform GetCurrentTarget()
    {
        if (players.Length == 0 || players[currentPlayerIndex] == null)
            return null;

        switchTimer -= Time.deltaTime;
        if (switchTimer <= 0f)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            switchTimer = switchTargetTime;
        }

        return players[currentPlayerIndex];
    }

    public void LookAtPlayer(Transform target)
    {
        if (target == null)
            return;

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

}
