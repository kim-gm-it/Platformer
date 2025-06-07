using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Player1Controller player1;
    public Player2Controler player2;

    public float damageCooldown = 1f;
    private Collider2D spikeCollider;

    //private float timeUntilNextHitP1 = 0f;
    //private float timeUntilNextHitP2 = 0f;

    private void Start()
    {
        spikeCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player1"))
        {
            player1.TakeDamage();
            Debug.Log("Player1 got hit by spike");
            StartCoroutine(DisableSpikeTemporarily());
            //timeUntilNextHitP1 = Time.time + damageCooldown;

        }

        if (collision.CompareTag("Player2"))
        {
            player2.TakeDamage();
            Debug.Log("Player2 got hit by spike");
            StartCoroutine (DisableSpikeTemporarily());
            //timeUntilNextHitP2 = Time.time + damageCooldown;

        }
    }

    private IEnumerator DisableSpikeTemporarily()
    {
        spikeCollider.enabled = false;
        yield return new WaitForSeconds(damageCooldown);
        spikeCollider.enabled = true;
    }
}
