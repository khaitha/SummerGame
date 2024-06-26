using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;    // Walking speed of the player
    [SerializeField] private float runSpeed;     // Running speed of the player
    [SerializeField] private float walkJumpPower = 5f;  // Jump power when walking
    [SerializeField] private float runJumpPower = 6f;   // Jump power when running

    private Rigidbody2D body;                    // Reference to the Rigidbody2D component
    private Animator anim;                       // Reference to the Animator component
    private bool grounded;                       // Whether the player is grounded
    private bool isJumping;                      // Whether the player is currently jumping
    private Vector3 originalScale;               // Original scale of the player
    public bool canMove = true;                 // Whether the player can move

    private void Awake()
    {
        // Initialize components
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        // Ensure gravity scale is reasonable and prevent character rotation
        body.gravityScale = 1f;
        body.freezeRotation = true;

        // Store the original scale
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!canMove) return; // Exit update if player can't move

        // Get horizontal input from the player (A/D keys or Left/Right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Determine the current speed based on whether the left shift key is pressed
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isJumping; // Check if not jumping to prioritize jump animation
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Set the Rigidbody2D velocity for movement
        body.velocity = new Vector2(horizontalInput * currentSpeed, body.velocity.y);

        // Flip the player's sprite based on the movement direction
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // Check if the space key is pressed and the player is grounded to perform a jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !isJumping)
            Jump(isRunning);

        // Update animator parameters to reflect movement and grounded states
        anim.SetBool("Move", horizontalInput != 0 && grounded); // Only consider horizontal movement when grounded
        anim.SetBool("grounded", grounded);
        anim.SetBool("Run", isRunning && grounded); // Only activate running animation when grounded
    }

    // Method to handle jumping
    private void Jump(bool isRunning)
    {
        // Set the Rigidbody2D velocity for the jump and trigger the jump animation
        float jumpPower = isRunning ? runJumpPower : walkJumpPower;
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("Jump");
        grounded = false;
        isJumping = true;
    }

    // Method to handle landing
    private void Land()
    {
        isJumping = false;
    }

    // Handle collision to determine if the player is grounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with an object tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            Land(); // Call the Land method when the player lands
        }
    }

    // Method to set the canMove variable
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
