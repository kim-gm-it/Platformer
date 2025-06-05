using UnityEngine;

public class ShootableTarget : MonoBehaviour
{
    public GameObject spikes;
    private bool wasHit = false;
    public float timer = 0;
    public float time = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        if (wasHit)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spikes.SetActive(true);
                wasHit = false;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player Arrow"))
        {
            Destroy(collision.gameObject);

            if (!wasHit) 
            {
                
                spikes.SetActive(false);
                timer = time;
                wasHit = true;
            }
            else
            {
                spikes.SetActive(true);
                wasHit = false;
            }
        }
    }
}
