using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections;

public class Player2Controler : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 40f;
    [SerializeField] int lives = 5;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;
    [SerializeField] Boolean player1;
    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public AudioClip jumpClip;
    public AudioClip attackClip;
    public AudioClip deathClip;
    private AudioSource audioSource;
    
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

        if ((collision.gameObject.tag == "Patrol Enemy")||(collision.gameObject.tag == "Patrol Enemy"))
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
}
