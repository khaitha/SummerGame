using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    // Limit for the camera position on the X-axis
    private float initialMinX = -41.79f;
    private float minX;
    private float maxForwardX = -41.79f;
    private float backwardReductionFactor = 5f; // Amount to reduce backward movement limit
    private float maxX = 34.36f; // Maximum X position for the camera

    private void Start()
    {
        minX = initialMinX;
    }

    private void Update()
    {
        // Update the max forward position
        if (player.position.x > maxForwardX)
        {
            maxForwardX = player.position.x;
            minX = maxForwardX - (Mathf.Floor((maxForwardX - initialMinX) / 30) * backwardReductionFactor);
        }

        // Calculate the new camera position
        float targetX = player.position.x + lookAhead;
        // Clamp the X position to ensure it doesn't go beyond the minimum and maximum X values
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Set the new position of the camera
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Update the lookAhead value
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
