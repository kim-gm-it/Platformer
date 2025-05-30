using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Hit player 1");
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Player2"))
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Hit player 2");

            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Ground"))
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Hit player ground");
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Player Arrow"))
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Hit player arrow");
            Destroy(gameObject);
        }
    }
}
