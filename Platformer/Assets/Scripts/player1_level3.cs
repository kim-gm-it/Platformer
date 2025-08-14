using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class player1_level3 : BasePlayer
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
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float arrowSpeed = 10f;

    [Header("Animation & Audio")]
    [SerializeField] Animator animator;
    [SerializeField] AudioClip attackClip, hitClip, deathClip;
    private AudioSource audioSource;

    // INPUTS
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
        animator.SetBool("Run", Mathf.Abs(movementInput.x) > 0f);

        if (movementInput.x > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (movementInput.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
    }
    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    public void ShootArrow()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        Quaternion rotation = direction > 0 ? Quaternion.identity : Quaternion.Euler(0, 0, 180f);

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(direction * arrowSpeed, 0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        
        if (collision.CompareTag("Boss") || collision.CompareTag("Patrol Enemy") )
        {
            TakeDamage();

            if (collision.CompareTag("Enemy Arrow"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        PlayHitSound();
        livesBar--;

        if (livesBar <= 0)
        {
            livesPoint--;
            animator.SetTrigger("Death");

            if (livesPoint > 0)
            {
                livesBar = 4;
            }

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
            FindFirstObjectByType<GameStateManagerlevel3>().PlayerDied(1);
        }
    }

    void UpdateHealthPointUI()
    {
        for (int i = 0; i < HealthPoint.Length; i++)
        {
            HealthPoint[i].sprite = (i < livesPoint) ? fullHeart : emptyHeart;
        }
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
