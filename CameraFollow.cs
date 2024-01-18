using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 5f; // camera turn speed
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        float desiredAngle = player.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

        Vector3 playerPosition = player.position;
        Vector3 desiredPosition = playerPosition + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(playerPosition);
    }
}