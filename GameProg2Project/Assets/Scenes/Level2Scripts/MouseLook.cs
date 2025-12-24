using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    float xRotation = 0f;
    void Start()
    {
        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera up/down (X axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 40f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player left/right (Y axis)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
