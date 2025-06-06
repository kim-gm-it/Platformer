using UnityEngine;

public class SkyBackground  : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        Vector3 newPos = cameraTransform.position;
        newPos.z = transform.position.z; 
        transform.position = newPos;
    }
}
