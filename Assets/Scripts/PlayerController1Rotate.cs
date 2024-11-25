using UnityEngine;

public class PlayerController1Rotate : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public float rotationSpeed = 100f; // Rotation speed
    public LayerMask groundLayer; // Layer for ground detection
    public Transform groundCheck; // Transform for ground check (empty GameObject at player's feet)

    public float groundCheckDistance = 0.1f; // Distance for raycast to check for ground

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right for strafing
        float moveZ = Input.GetAxis("Vertical"); // W/S or Up/Down for forward/backward

        Vector3 move = new Vector3(moveX, 0, moveZ) * speed;
        move = transform.TransformDirection(move); // Adjust movement relative to player's direction
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x;
        velocity.z = move.z;
        rb.linearVelocity = velocity;

        // Rotation input (Left/Right or A/D)
        float rotateInput = Input.GetAxis("Horizontal"); // A/D or Left/Right for rotation
        float rotation = rotateInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0); // Rotate the player around Y-axis

         // Jump input
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize raycast for ground check in editor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
