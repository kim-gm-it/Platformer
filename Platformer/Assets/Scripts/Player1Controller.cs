using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class Player1Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 40f;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;
    [SerializeField] int lives = 5;
    private float doubleJampPower = 15f;
    private bool canDoubleJump;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrowPrefab;
    public AudioClip jumpClip;
    public AudioClip attackClip;
    public AudioClip deathClip;

    private AudioSource audioSource;

    // [SerializeField] private float arrowSpeed = 10f;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                GetComponent<Player1Controller>().PlayJumpSound();
                isGrounded = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.AddForce(Vector2.up * doubleJampPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                GetComponent<Player1Controller>().PlayJumpSound();
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            animator.SetTrigger("GetHit");
            lives -= 1;
            if (lives <= 0)
            {
                animator.SetTrigger("Death");
                GetComponent<Player2Controler>().PlayDeathSound();
            }
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            GetComponent<Player1Controller>().PlayAttackSound();
            // ShootArrow();
        }
    }
    // private void ShootArrow()
    // {
    //     GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
    //     Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
    //     float direction = transform.localScale.x > 0 ? 1f : -1f;
    //     rb.AddForce(new Vector2(direction * arrowSpeed, 0f), ForceMode2D.Impulse);

    // }
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
}
