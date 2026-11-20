
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraRotationX = 15f;
    private float cameraRotationY;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        cameraRotationY = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Управление камерой
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraRotationY += mouseX;
        cameraRotationX -= mouseY;

        cameraRotationX = Mathf.Clamp(cameraRotationX, -20f, 70f);

        // Передвижение относительно камеры
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * z + right * x;
        movement = Vector3.ClampMagnitude(movement, 1f);

        // Поворот персонажа
        if (movement.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movement),
                10f * Time.deltaTime
            );
        }

        // Прыжок
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
                verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity =
            movement * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        // Разблокировка мыши
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        // Позиционирование камеры позади персонажа
        Quaternion rotation = Quaternion.Euler(
            cameraRotationX,
            cameraRotationY,
            0f
        );

        Vector3 target = transform.position +
                         Vector3.up * cameraHeight;

        Vector3 cameraPosition = target -
                                 rotation * Vector3.forward *
                                 cameraDistance;

        cameraTransform.position = cameraPosition;
        cameraTransform.LookAt(target);
    }
}
