using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float sensitivity = 100f; // Mouse sensitivity for horizontal rotation
    public Transform playerBody; // Reference to the player's body for rotation

    void Update()
    {
        // Get mouse movement on the X-axis (left and right)
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // Rotate the player body around the Y-axis (horizontal rotation)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
