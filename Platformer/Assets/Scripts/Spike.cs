using UnityEngine;

public class Spike : MonoBehaviour
{
    public Player1Controller player1;
    public Player2Controler player2;

    public float damageCooldown = 1f;

    private float timeUntilNextHitP1 = 0f;
    private float timeUntilNextHitP2 = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player1") && Time.time >= timeUntilNextHitP1)
        {
            player1.TakeDamage();
            Debug.Log("Player1 got hit by spike");
            timeUntilNextHitP1 = Time.time + damageCooldown;

        }

        if (collision.CompareTag("Player2") && Time.time >= timeUntilNextHitP2)
        {
            player2.TakeDamage();
            Debug.Log("Player2 got hit by spike");
            timeUntilNextHitP2 = Time.time + damageCooldown;

        }
    }
}
