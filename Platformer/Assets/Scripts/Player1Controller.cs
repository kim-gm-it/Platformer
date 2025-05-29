using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player1Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 40f;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;
    [SerializeField] Boolean player2;
    [SerializeField] int livesPoint = 3;
    [SerializeField] int livesBar = 4;
    [SerializeField] float doubleJampPower = 25f;
    private bool canDoubleJump;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrowPrefab;
    public AudioClip jumpClip;
    public AudioClip attackClip;
    public AudioClip deathClip;
    private AudioSource audioSource;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 10f;
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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isOnPlatform = isGrounded || player2;

            if (isOnPlatform)
            {
                rb.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                PlayJumpSound();
                isGrounded = false;
                player2 = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.AddForce(Vector2.up * doubleJampPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                PlayJumpSound();
                canDoubleJump = false;
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
    }

    void Update()
    {
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Player2"))
        {
            player2 = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Player2"))
        {
            player2 = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            if (collision.gameObject.tag == "EnemyProjectile")
            {
                animator.SetTrigger("GetHit");
                livesBar--;

                UpdateHealthBarUI();

                if (livesBar <= 0)
                {
                    livesPoint--;
                    livesBar = 3;
                    UpdateHealthPointUI();
                    UpdateHealthBarUI();
                }

                if (livesPoint <= 0)
                {
                    animator.SetTrigger("Death");
                    PlayDeathSound();
                    losePanel.SetActive(true);
                }

                Destroy(collision.gameObject);
            }
        }


    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            GetComponent<Player1Controller>().PlayAttackSound();
        }
    }
    public void ShootArrow()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(direction * arrowSpeed, 0f);

        if (direction < 0)
        {
            arrow.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpClip);
    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }
    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClip);
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
}
