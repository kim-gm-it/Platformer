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
    public float attackRange = 7f;
    public float attackCooldown = 3f;
    public float detectionRange = 10f;
    public float radius = 0.5f;
    public LayerMask playerLayer;
    public GameObject attackPoint;

    private float attackTimer = 0f;
    private GameObject[] players;
    private EnemyLogic enemyLogic;
    private Transform targetPlayer;
    private bool isChasing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //attackPoint = GameObject.FindGameObjectWithTag("AttackPoint");
        if (attackPoint == null)
        {
            Debug.Log("Attack point not assigned yet");
        }
        enemyLogic = GetComponent<EnemyLogic>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentPoint = pointB.transform;
        animator.SetBool("IsRunning" , true);

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
            Debug.Log("chasing player: " + targetPlayer.name);
            chasePlayer();
        }
        else
        {
            Debug.Log("patrolling");
            patrol();
        }

    }

    public void patrol()
    {
        Debug.Log("Enemy is patrolling towards: " + currentPoint.name);
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0f);
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
        animator.SetBool("IsRunning" , true);  
    }

    public void chasePlayer()
    {
        float direction = Mathf.Sign(targetPlayer.position.x - transform.position.x);

        float targetx = transform.position.x + direction * speed * Time.deltaTime;

        //movement limited to patrol boundaries
        if((direction<0 && targetx < pointA.transform.position.x ) || (direction > 0 && targetx > pointB.transform.position.x))
        {
            Debug.Log("chase blocked by patrol boundary. switch to patrolling .");
            patrol();
            return;
        }

        rb.velocity = new Vector2(direction * speed , rb.velocity.y);

        if((direction > 0  && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            flip();
        }

        animator.SetBool("IsRunning", true);

        //Attack if in range

        float distance = Mathf.Abs(transform.position.x - targetPlayer.transform.position.x);
        if(distance <= attackRange && attackTimer <= 0f)
        {
            Debug.Log("Enemy attacking");
            enemyLogic.OnAttack();
            attackTimer = attackCooldown;
        }
    }

    public void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, playerLayer);
        foreach(Collider2D player in players)
        {
            Debug.Log("Hit player\n");
            if(player.tag == "Player2")
            {
                player.GetComponent<Player2Controler>().TakeDamage();
            }
            else if (player.tag == "Player1")
            {
                player.GetComponent<Player1Controller>().TakeDamage();
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
