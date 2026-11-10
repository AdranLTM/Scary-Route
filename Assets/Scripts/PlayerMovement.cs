using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;

    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;

    [Header("Stamina")]
    public float maxStamina = 10f;
    public float staminaDrainRate = 1f;
    public float staminaRechargeRate = 2f;
    public float staminaRechargeDelay = 1f;

    private Vector3 velocity;

    private float currentStamina;
    private float rechargeTimer;

    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsSprinting { get; private set; }

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Kontrola zemì
        IsGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // WASD
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        IsMoving = move.magnitude > 0.1f;

        // Sprint
        bool wantsToSprint =
            Input.GetKey(KeyCode.LeftShift) &&
            IsMoving &&
            currentStamina > 0f;

        if (wantsToSprint)
        {
            IsSprinting = true;

            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            rechargeTimer = staminaRechargeDelay;
        }
        else
        {
            IsSprinting = false;

            if (rechargeTimer > 0f)
            {
                rechargeTimer -= Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
        }

        float speed = IsSprinting ? sprintSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        // Skok
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravitace
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}