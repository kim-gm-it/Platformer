using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float detectionRange = 10f;
    public float shootingInterval = 1.5f;
    public float shootingSpeed  =15f;
    public float shootingTimer;
    public AudioClip shootingSound;
    private AudioSource audioSource;
    private Animator animator;

    private Transform targetPlayer;
    GameObject[] players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = new GameObject[2];
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTarget();

        if (targetPlayer == null)
            return;

       
        shootingTimer += Time.deltaTime;

        if(shootingTimer >= shootingInterval) //if enough time has passed shoot a bullet and reset timer
        {
            Shoot();
            animator.SetTrigger("IsAttacking");
            PlayShootingSound();
            shootingTimer = 0;  
        }

        //flip based on tartget position
        if (targetPlayer != null)
        {
            Vector3 scale = transform.localScale;

            if (targetPlayer.position.x > transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            else
                scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }


    }


    public void updateTarget()
    {
        if(players[0] == null)
        {
            players[0] = GameObject.FindGameObjectWithTag("Player1");
        }
        if (players[1] == null)
        {
            players[1] = GameObject.FindGameObjectWithTag("Player2");
        }

        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach(GameObject player in players)
        {
            if(player == null) continue;

            float distance = Mathf.Abs(transform.position.x - player.transform.position.x);
            if (distance <= detectionRange && distance < closestDistance) { }
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        targetPlayer = closestPlayer;
    }
    //public void findTarget()
    //{
    //    players[0] = GameObject.FindGameObjectWithTag("Player1");
    //    players[1] = GameObject.FindGameObjectWithTag("Player2");
    //    for(int i=0; i<players.Length ; i++)
    //    {
    //        if(Mathf.Abs(transform.position.x - players[i].transform.position.x) <= detectionRange)
    //        {
    //            targetPlayer = players[i].transform;
    //            break;
    //        }
    //    }
    //}

    public void Shoot()
    {
        if(bulletPrefab != null && targetPlayer != null)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;//to keep the direction only , and not the elength
            GameObject bullet = Instantiate(bulletPrefab , transform.position , Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * shootingSpeed;
        }
    }

    public void PlayShootingSound()
    {
        audioSource.PlayOneShot(shootingSound);
    }
}
