using UnityEngine;

// Движение по двум осям для top-down 2D-игр.
public class TopDownMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Joystick joystick;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");

        float joystickX = joystick != null ? joystick.Horizontal : 0f;
        float joystickY = joystick != null ? joystick.Vertical : 0f;

        movement = new Vector2(
            Mathf.Clamp(keyboardX + joystickX, -1f, 1f),
            Mathf.Clamp(keyboardY + joystickY, -1f, 1f)
        ).normalized;
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
}
