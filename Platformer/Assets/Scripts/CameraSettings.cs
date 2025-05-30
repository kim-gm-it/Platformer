using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [Header("Player's transform refs")]
    public Transform player;

    [Header("camera settings")]
    public float lookAheadDistance = 3f;// how far ahead a camera should look
    public float followSpeed = 5f;//camera following speed

    private Vector3 targetpos;//where the camera wants to move
    private float lastPlayerX;//for movement detection

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPlayerX = player.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = player.position.x - lastPlayerX;// calculating players movement(left/right)

        //look ahead based on direction
        float lookAhead =  moveDirection > 0 ? lookAheadDistance : -lookAheadDistance;

        // if player barely moves
        if(Mathf.Abs(moveDirection) <= 0.01f)
        {
            lookAhead = 0;
        }

        targetpos = new Vector3(player.position.x + lookAhead, player.position.y, transform.position.z);// build a pos ahead of the player in their moving direction while keeping the current z 

        transform.position = Vector3.Lerp(transform.position, targetpos, followSpeed * Time.deltaTime);//Vector3.Lerp is for smooth movement 

        lastPlayerX = player.position.x;//updating x position 
    }
}
