using UnityEngine;

public class BounceMarkerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Pitch Boundaries")]
    public float minX = -1.5f;
    public float maxX = 1.5f;
    public float minZ = -5f;
    public float maxZ = 5f;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
       
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");   

        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        newPosition.y = 0.01f;

      
        transform.position = newPosition;
    }
}