using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class player2_level3 : BasePlayer
{
    private Vector3 initialScale;
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    private Vector2 movementInput;

    [Header("Health System")]
    
    // [SerializeField] int livesPoint = 3;
    // [SerializeField] int livesBar = 4;
    [SerializeField] Image[] HealthPoint;
    [SerializeField] Image[] HealthBar;
    [SerializeField] Sprite fullHeart, emptyHeart;
    [SerializeField] Sprite Bar4, Bar3, Bar2, Bar1, Bar0;
    [SerializeField] GameObject losePanel;
    private bool isDead = false;

    [Header("Combat")]
    //[SerializeField] GameObject attackPoint;
    [SerializeField] Vector3 attackOffset;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float radius = 1.1f;
    [SerializeField] LayerMask enemyLayer;
    //[SerializeField] GameObject damageDrop;

    [Header("Animation & Audio")]
    [SerializeField] Animator animator;
    [SerializeField] AudioClip attackClip, hitClip, deathClip;
    private AudioSource audioSource;

    private int hitCount = 0;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            PlayAttackSound();
            Attack();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale;
    }
    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
    void Update()
    {
    
        animator.SetBool("Run", movementInput.magnitude > 0f);

        if (movementInput.x > 0.01f)
        transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (movementInput.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        //|| collision.CompareTag("Patrol Enemy")
        if (collision.CompareTag("Boss") )
        {
            TakeDamage();
            if (collision.CompareTag("Enemy Arrow"))
                Destroy(collision.gameObject);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        PlayHitSound();
        hitCount++;

        if (hitCount >= 1)
        {
            hitCount = 0;
            livesBar--;

            if (livesBar <= 0)
            {
                livesPoint--;
                animator.SetTrigger("Death");
                if (livesPoint > 0) livesBar = 4;
                UpdateHealthPointUI();
            }
            else
            {
                animator.SetTrigger("GetHit");
            }

            UpdateHealthBarUI();

            if (livesPoint <= 0)
            {
                isDead = true;
                PlayDeathSound();
                StartCoroutine(ShowLosePanelAfterDelay(1.5f));
                FindFirstObjectByType<GameStateManagerlevel3>().PlayerDied(2);
            }
        }
        else
        {
            animator.SetTrigger("GetHit");
        }
    }

    public void Attack()
    {
        Vector3 currentOffset = attackOffset;
        currentOffset.x *= transform.localScale.x;
        Vector3 pos = currentOffset + transform.position;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(pos, attackRange , enemyLayer);

        Debug.Log(enemies.Length + " enemies detected.");

        foreach (Collider2D enemy in enemies)
        {
            Boss boss = enemy.GetComponent<Boss>();

            if (boss != null && !boss.IsDead())
            {
                //int damage = damageDrop.GetComponent<CollectibleItem>().getDamageCapacity();
                boss.TakeDamage(4);
                continue;
            }

            EnemyLogicLevel3 enemyLogic = enemy.GetComponent<EnemyLogicLevel3>();

            if(enemyLogic != null)
            {
                enemyLogic.TakeDamage(1);
                continue;
            }
        }
}

    private void OnDrawGizmos()
    {
        Vector3 currentOffset = attackOffset;
        currentOffset.x *= transform.localScale.x;
        Vector3 pos = currentOffset + transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, radius);
    }

    void UpdateHealthPointUI()
    {
        for (int i = 0; i < HealthPoint.Length; i++)
            HealthPoint[i].sprite = (i < livesPoint) ? fullHeart : emptyHeart;
    }

    void UpdateHealthBarUI()
    {
        for (int i = 0; i < HealthBar.Length; i++)
        {
            switch (livesBar)
            {
                case 4: HealthBar[i].sprite = Bar4; break;
                case 3: HealthBar[i].sprite = Bar3; break;
                case 2: HealthBar[i].sprite = Bar2; break;
                case 1: HealthBar[i].sprite = Bar1; break;
                case 0: HealthBar[i].sprite = Bar0; break;
            }
        } 
    }

    private IEnumerator ShowLosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        // FindFirstObjectByType<GameStateManager>().ShowGameOver();
    }

    void PlayAttackSound() => audioSource.PlayOneShot(attackClip);
    void PlayHitSound() => audioSource.PlayOneShot(hitClip);
    void PlayDeathSound() => audioSource.PlayOneShot(deathClip);
}
