using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForse = 5f;
    private Vector2 movementInput;
    [SerializeField] Boolean isGrounded;

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Input: " + context.ReadValue<Vector2>());
        if (context.performed && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up*jumpForse , ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move Input: " + context.ReadValue<Vector2>());
        movementInput = context.ReadValue <Vector2>();
    }
    void Start()
    {
        
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
