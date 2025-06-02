using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum CollectibleType { Health, Damage }
    public CollectibleType itemType;

    public int healthAmount = 1;
    public float damageMultiplier = 2f;
    public float damageDuration = 5f;

    private AudioSource audioSource;
    private bool collected = false;
    [SerializeField] int damageCapacity = 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            var player1 = other.GetComponent<Player1Controller>();
            var player2 = other.GetComponent<Player2Controler>();

            if (player1 != null || player2 != null)
            {
                collected = true;

                if (itemType == CollectibleType.Health)
                {
                    if (player1 != null) player1.IncreaseHealth(healthAmount);
                    if (player2 != null) player2.IncreaseHealth(healthAmount);
                }
                else if (itemType == CollectibleType.Damage)
                {
                    DamageBoostUIManager.instance.StartBoostTimer(damageDuration);
                    damageCapacity = 3;
                }

                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    GetComponent<SpriteRenderer>().enabled = false; 
                    GetComponent<Collider2D>().enabled = false;     
                    Destroy(gameObject, audioSource.clip.length);  
                }
                else
                {
                    Destroy(gameObject); 
                }
            }
        }
    }
}
