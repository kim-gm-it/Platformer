using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;


public class Player2Controler : MonoBehaviour
{
    private Vector3 initialScale;
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 40f;
    [SerializeField] int livesPoint = 3;
    [SerializeField] int livesBar = 4;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;
    [SerializeField] Boolean player1;
    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    public bool hasKey = false;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public AudioClip jumpClip;
    public AudioClip attackClip;
    public AudioClip deathClip;
    public AudioClip hitClip;
    private AudioSource audioSource;
    public Image[] HealthPoint;
    public Image[] HealthBar;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite Bar4;
    public Sprite Bar3;
    public Sprite Bar2;
    public Sprite Bar1;
    public Sprite Bar0;
    public GameObject losePanel;
    private bool isDead = false;
    private int hitCount = 0;

    public void OnJump(InputAction.CallbackContext context)
    {
        if ((context.performed && isGrounded) || (context.performed && player1))
        {
            rb.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            GetComponent<Player2Controler>().PlayJumpSound();
            if (isGrounded)
            {
                isGrounded = false;
            }
            if (player1)
            {
                player1 = false;
            }

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move Input: " + context.ReadValue<Vector2>());
        movementInput = context.ReadValue<Vector2>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        Vector2 movement = new Vector2(movementInput.x, movementInput.y) * (moveSpeed * Time.deltaTime);
        if (movement.magnitude > 0f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        transform.Translate(movement);
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (movementInput.x > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else if (movementInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Player1"))
        {
            player1 = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Player1"))
        {
            player1 = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
{
    if (isDead) return;

    if ((collision.gameObject.tag == "Patrol Enemy") || (collision.gameObject.tag == "Enemy Arrow"))
    {
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
                StartCoroutine(ShowLosePanelAfterDelay(1.9f));
            }
        }
        else
        {
            animator.SetTrigger("GetHit"); 
        }

        if(collision.gameObject.tag == "Enemy Arrow"){
            Destroy(collision.gameObject);
        }
    }
}


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            GetComponent<Player2Controler>().PlayAttackSound();
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpClip, 1.5f);
    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }
    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClip);
    }
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitClip);
    }
    void UpdateHealthPointUI()
    {
        for (int i = 0; i < HealthPoint.Length; i++)
        {
            if (i < livesPoint)
            {
                HealthPoint[i].sprite = fullHeart;
            }
            else
            {
                HealthPoint[i].sprite = emptyHeart;
            }
        }
    }
    void UpdateHealthBarUI()
    {
        for (int i = 0; i < HealthBar.Length; i++)
        {
            switch (livesBar)
            {
                case 4:
                    HealthBar[i].sprite = Bar4;
                    break;
                case 3:
                    HealthBar[i].sprite = Bar3;
                    break;
                case 2:
                    HealthBar[i].sprite = Bar2;
                    break;
                case 1:
                    HealthBar[i].sprite = Bar1;
                    break;
                case 0:
                    HealthBar[i].sprite = Bar0;
                    break;
            }
        }
    }
    public void IncreaseHealth(int amount)
    {
        livesBar += amount;

        if (livesBar > 4)
        {
            livesBar = 4;
        }

        UpdateHealthBarUI();
    }

    public void StartDamageBoost(float multiplier, float duration)
    {
        StartCoroutine(DamageBoostCoroutine(multiplier, duration));
    }

    private IEnumerator DamageBoostCoroutine(float multiplier, float duration)
    {
        moveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed /= multiplier;
    }
    private IEnumerator ShowLosePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindFirstObjectByType<GameStateManager>().ShowGameOver();
    }
}