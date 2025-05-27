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
    private float doubleJampPower = 15f;
    private bool canDoubleJump;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrowPrefab;
    // [SerializeField] private float arrowSpeed = 10f;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                isGrounded = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.AddForce(Vector2.up * doubleJampPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
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
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
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
}
