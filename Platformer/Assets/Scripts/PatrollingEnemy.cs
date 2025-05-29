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


    [Header("Attack related variables")]
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    private float attackTimer = 0f;
    private GameObject[] players;
    private EnemyLogic enemyLogic;


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


        attackTimer -= Time.deltaTime;

        for(int i=0; i<players.Length; i++)
        {
            if (players[i] == null)
                continue;
            float distance = Mathf.Abs(transform.position.x - players[i].transform.position.x);
            if (distance <= attackRange && attackTimer >= attackCooldown)
            {
                enemyLogic.OnAttack();
                attackTimer = 0;
            }
        }

    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
