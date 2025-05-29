using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float detectionRange = 10f;
    public float shootingInterval = 1.5f;
    public float shootingSpeed  =7f;
    public float shootingTimer;

    private Transform targetPlayer;
    GameObject[] players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer == null)
        {
            findTarget();
            return;
        }

        shootingTimer += Time.deltaTime;

        if(shootingTimer >= shootingInterval) //if enough time has passed shoot a bullet and reset timer
        {
            Shoot();
            shootingTimer = 0;  
        }
    }

    public void findTarget()
    {
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        for(int i=0; i<players.Length ; i++)
        {
            if(Mathf.Abs(transform.position.x - players[i].transform.position.x) <= detectionRange)
            {
                targetPlayer = players[i].transform;
                break;
            }
        }
    }

    public void Shoot()
    {
        if(bulletPrefab != null && targetPlayer != null)
        {

        }
    }
}
