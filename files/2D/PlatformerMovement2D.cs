using UnityEngine;


public class PlatformerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Joystick joystick;

    [Header("Jump")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveX;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float joystickX = joystick != null ? joystick.Horizontal : 0f;
        moveX = Mathf.Clamp(keyboardX + joystickX, -1f, 1f);

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        bool isGrounded = groundCheck != null &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundRadius,
                groundLayer
            );

        if (jumpRequested && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        jumpRequested = false;
    }

    public void Jump()
    {
        jumpRequested = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
