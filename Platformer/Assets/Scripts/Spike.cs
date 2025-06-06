using UnityEngine;

public class Spike : MonoBehaviour
{
    public Player1Controller player1;
    public Player2Controler player2;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Debug.Log("Hit players");
            player1.TakeDamage();
            player2.TakeDamage();
        }
    }
}
