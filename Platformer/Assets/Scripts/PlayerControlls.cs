using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlls : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 20f;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;
    private float doubleJampPower=15f;

    private bool canDoubleJump;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
                isGrounded = false;
                canDoubleJump = true; 
            }
            else if (canDoubleJump)
            {
                rb.AddForce(Vector2.up * doubleJampPower, ForceMode2D.Impulse);
                canDoubleJump = false; 
            }
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move Input: " + context.ReadValue<Vector2>());
        movementInput = context.ReadValue <Vector2>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = new Vector2(movementInput.x , movementInput.y)*(moveSpeed * Time.deltaTime);
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
}
