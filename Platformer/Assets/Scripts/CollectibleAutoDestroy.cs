using UnityEngine;

public class CollectibleAutoDestroy : MonoBehaviour
{
    public float destroyDistance = 20f; 

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, Camera.main.transform.position) > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}