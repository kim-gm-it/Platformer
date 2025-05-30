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
    
    [Header("Attack related variables")]
    public float speed = 3f;
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    public float detectionRange = 10f;

    private float attackTimer = 0f;
    private GameObject[] players;
    private EnemyLogic enemyLogic;
    private Transform targetPlayer;
    private bool isChasing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentPoint = pointB.transform;
        animator.SetBool("is Running" , true);

        players = new GameObject[2];
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        float closestDistance = Mathf.Infinity;
        targetPlayer = null; 
        //find closest player in detection range
        for(int i=0; i<players.Length; i++)
        {
            if (players[i] == null)
                continue;
            float distance = Mathf.Abs(transform.position.x - players[i].transform.position.x);
            if(distance <= detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                targetPlayer = players[i].transform;

            }

        }

        isChasing = targetPlayer != null;

        if (isChasing)
        {
            chasePlayer();
        }
        else
        {
            patrol();
        }

    }

    public void patrol()
    {
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0f);
        }

        if (Mathf.Abs(transform.position.x - currentPoint.position.x) <= 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if (Mathf.Abs(transform.position.x - currentPoint.position.x) <= 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
        animator.SetBool("is Running" , true);  
    }

    public void chasePlayer()
    {
        float direction = Mathf.Sign(targetPlayer.position.x - transform.position.x);
        float targetx = transform.position.x + direction * speed * Time.deltaTime;

        //movement limited to patrol boundaries
        if((direction<0 && targetx < pointA.transform.position.x ) || (direction > 0 && targetx > pointB.transform.position.x))
        {
            patrol();
            return;
        }

        rb.velocity = new Vector2(direction * speed , rb.velocity.y);
        if((direction > 0  && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            flip();
        }

        animator.SetBool("is Running", true);

        //Attack if in range

        float distance = Mathf.Abs(transform.position.x - targetPlayer.transform.position.x);
        if(distance <= attackRange && attackTimer <= 0f)
        {
            enemyLogic.OnAttack();
            attackTimer = attackCooldown;
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
