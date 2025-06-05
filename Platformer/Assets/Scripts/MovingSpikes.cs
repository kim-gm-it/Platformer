using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public float moveDis = 1f;
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    private Vector3 startPos;
    private bool movingUp= false;
    private float waitTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTimer > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        float direction = movingUp ? 1f : -1f;

        //moving up or down based on direction
        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;

        //if spike reached the top point start timer 
        if(movingUp && transform.position.y >= startPos.y + moveDis)
        {
            transform.position = new Vector3(transform.position.x , startPos.y + moveDis , transform.position.z);
            movingUp = false;
            waitTimer = waitTime;
        }
        //if spike reached the bottom point start timer
        if (!movingUp && transform.position.y <= startPos.y)
        {
            transform.position = new Vector3(transform.position.x, startPos.y , transform.position.z);
            movingUp = true;
            waitTimer = waitTime;
        }
    }
}
