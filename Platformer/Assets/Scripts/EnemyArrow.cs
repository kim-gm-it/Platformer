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
            Debug.Log("Hit player 1");
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Destroy(gameObject, 0.1f);
        }

        else if (collision.CompareTag("Player2"))
        {
            Debug.Log("Hit player 2");
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            Destroy(gameObject, 0.1f);
        }

        else if (collision.CompareTag("Ground"))
        {
            Debug.Log("Hit player ground");
            Destroy(gameObject, 0.1f);
        }

        else if (collision.CompareTag("Player Arrow"))
        {
            Debug.Log("Hit player arrow");
            Destroy(gameObject, 0.1f);
        }
    }
}
