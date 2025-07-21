using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class player2_level3 : MonoBehaviour
{
    private Vector3 initialScale;
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    private Vector2 movementInput;

    [Header("Health System")]
    [SerializeField] int livesPoint = 3;
    [SerializeField] int livesBar = 4;
    [SerializeField] Image[] HealthPoint;
    [SerializeField] Image[] HealthBar;
    [SerializeField] Sprite fullHeart, emptyHeart;
    [SerializeField] Sprite Bar4, Bar3, Bar2, Bar1, Bar0;
    [SerializeField] GameObject losePanel;
    private bool isDead = false;

    [Header("Combat")]
    [SerializeField] GameObject attackPoint;
    [SerializeField] float radius = 0.9f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject damageDrop;

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

    void Update()
    {
        Vector2 movement = movementInput * (moveSpeed * Time.deltaTime);
        transform.Translate(movement);

        animator.SetBool("Run", movement.magnitude > 0f);

        if (movementInput.x > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (movementInput.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Boss"))
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

        if (hitCount >= 2)
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
                StartCoroutine(ShowLosePanelAfterDelay(1.9f));
            }
        }
        else
        {
            animator.SetTrigger("GetHit");
        }
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Boss boss = enemy.GetComponent<Boss>();
            if (boss != null && !boss.IsDead())
            {
                int damage = damageDrop.GetComponent<CollectibleItem>().getDamageCapacity();
                boss.TakeDamage(damage);
            }
        }
}

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
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
        FindFirstObjectByType<GameStateManager>().ShowGameOver();
    }

    void PlayAttackSound() => audioSource.PlayOneShot(attackClip);
    void PlayHitSound() => audioSource.PlayOneShot(hitClip);
    void PlayDeathSound() => audioSource.PlayOneShot(deathClip);
}
