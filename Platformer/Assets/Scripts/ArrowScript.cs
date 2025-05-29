using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public AudioClip hitSound; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Patrol Enemy"))
        {
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }            
            Destroy(gameObject, 0.1f);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
