using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyChase : MonoBehaviour
{
    GameObject[] players;
    public float speed = 3f ;
    public float detectionRange = 10f;
    private float distance;
    private Transform targetPlayer;
    

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
        
        distance = Mathf.Abs(transform.position.x - targetPlayer.transform.position.x);
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;


    }

    public void findTarget()
    {
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        for(int i=0; i < players.Length; i++)
        {
            if(Mathf.Abs(transform.position.x - players[i].transform.position.x) <= detectionRange)
            {
                targetPlayer = players[i].transform;
                break;
            }
        }
    }
}
