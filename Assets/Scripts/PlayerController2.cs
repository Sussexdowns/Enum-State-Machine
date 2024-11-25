using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public LayerMask groundLayer; // Layer for ground detection
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
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical"); // W/S or Up/Down

        Vector3 move = new Vector3(moveX, 0, moveZ) * speed;
        move = transform.TransformDirection(move); // Adjust movement relative to player's direction
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x;
        velocity.z = move.z;
        rb.linearVelocity = velocity;

        // Ground check using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Jump input
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
