using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class MinionsMovement : MonoBehaviour
{
    [Header("Chase And Attack Variables")]
    public float attackCooldown = 2f;
    public float attackRange = 2f;
    public float radius = 0.5f;
    public float speed = 3f;

    private LayerMask playerLayer;
    private GameObject attackPoint;
    private float attackTimer;
    private GameObject[] players;
    private EnemyLogic enemyLogic;
    private Transform target;
    private bool isChasing = false;
    private bool isAttacking = false;
    private Animator animator;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        playerLayer = LayerMask.GetMask("Player");

        Debug.Log("LayerMask value: " + playerLayer.value);

        attackPoint = GameObject.Find("AttackPoint");

        if (attackPoint == null)
        {
            Debug.Log("Attack point not assigned");
        }

        enemyLogic = GetComponent<EnemyLogic>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsRunning", true);

        players = new GameObject[2];
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
    }


    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        float closestDis = Mathf.Infinity;
        target = null;

        //Find the closest player
        float playerX, playerY, enemyX, enemyY;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                continue;
            }
            
            playerX = players[i].transform.position.x;
            playerY = players[i].transform.position.y;
            enemyX = transform.position.x;
            enemyY = transform.position.y;

            float distance = Mathf.Pow(playerX - enemyX,2) + Mathf.Pow(playerY - enemyY , 2);

            if(distance < closestDis)
            {
                closestDis = distance;
                target = players[i].transform;
            }
        }

        isChasing = target != null;

        if (isChasing)
        {
            ChasePlayer();
        }
        if (isAttacking)
        {
            Attack();
            isAttacking = false;
        }

    }

    private void ChasePlayer()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * speed;

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0  && transform.localScale.x > 0))
        {
            flip();
        }

        animator.SetBool("IsRunning", true);

        float distance = Vector2.Distance(transform.position, target.position);

        if(distance <= attackRange && attackTimer <= 0)
        {
            attackTimer = attackCooldown;
            isAttacking = true;
            enemyLogic.OnAttack();
        }
    }

    private void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, playerLayer);
        
        Debug.Log(players.Length + "players detected.");

        foreach(Collider2D player in players)
        {
            Debug.Log("Enemy is hitting " + player.name);

            if (player.CompareTag("Player1"))
            {
                player.GetComponent<player1_level3>().TakeDamage();
                
            }else if (player.CompareTag("Player2"))
            {
                player.GetComponent<player2_level3>().TakeDamage();

            }
        }


    }

    private void OnDrawGizmos()
    {
        if(attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.transform.position , radius);
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}
