using UnityEngine;

public class Boundries : MonoBehaviour
{
    private Collider2D collider;
    private GameObject boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        Collider2D bossCollider = boss.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, bossCollider);
    }

}
