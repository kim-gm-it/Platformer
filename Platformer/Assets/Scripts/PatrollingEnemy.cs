using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        animator.SetBool("isRunning" , true);

    }

    // Update is called once per frame
    void Update()
    {


        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed , 0f);
        }
        else
        {
            rb.velocity = new Vector2(-speed , 0f);
        }

        if( Mathf.Abs(transform.position.x - currentPoint.position.x)  <= 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if (Mathf.Abs(transform.position.x - currentPoint.position.x) <= 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
