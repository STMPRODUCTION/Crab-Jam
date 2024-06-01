using UnityEngine;

public class CrabCameraController : MonoBehaviour
{
    public Transform crabTransform; // Reference to the crab's transform
    public float distance = 5.0f; // Distance from the crab
    public float mouseSensitivity = 100.0f; // Mouse sensitivity for rotation

    private float currentYaw = 0.0f;
    private float currentPitch = 0.0f;

    void Start()
    {
        // Set initial position and rotation
        Vector3 initialPosition = crabTransform.position - transform.forward * distance;
        transform.position = initialPosition;
    }

    void Update()
    {
        // Check for right mouse button input
        if (Input.GetMouseButton(1))
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Adjust the yaw and pitch based on the mouse input
            currentYaw += mouseX;
            currentPitch -= mouseY;

            // Clamp the pitch to prevent flipping
            currentPitch = Mathf.Clamp(currentPitch, -35, 60);
        }

        // Calculate the new rotation
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0.0f);

        // Calculate the new position
        Vector3 position = crabTransform.position - rotation * Vector3.forward * distance;

        // Set the camera's position and rotation
        transform.position = position;
        transform.LookAt(crabTransform);
    }
}
