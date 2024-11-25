using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public LayerMask groundLayer; // Layer for ground detection
    public Transform groundCheck; // Transform for ground check (empty GameObject at player's feet)
    public float groundCheckRadius = 0.2f; // Radius for ground check sphere

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

        // Jump input
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check radius in editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
