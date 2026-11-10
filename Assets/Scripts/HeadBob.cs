using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public PlayerMovement playerMovement;

    [Header("Walking")]
    public float walkBobSpeed = 10f;
    public float walkBobAmount = 0.06f;

    [Header("Sprinting")]
    public float sprintBobSpeed = 16f;
    public float sprintBobAmount = 0.09f;

    [Header("Smooth")]
    public float smoothSpeed = 15f;

    private Vector3 originalPosition;
    private float timer;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (playerMovement == null)
            return;

        // Kývání vždy pøi pohybu
        if (playerMovement.IsMoving && playerMovement.IsGrounded)
        {
            float speed;
            float amount;

            if (playerMovement.IsSprinting)
            {
                speed = sprintBobSpeed;
                amount = sprintBobAmount;
            }
            else
            {
                speed = walkBobSpeed;
                amount = walkBobAmount;
            }

            timer += Time.deltaTime * speed;

            Vector3 newPosition = originalPosition;

            // nahoru / dolù
            newPosition.y += Mathf.Sin(timer) * amount;

            // lehce vlevo / vpravo
            newPosition.x += Mathf.Cos(timer * 0.5f) * amount * 0.5f;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                newPosition,
                Time.deltaTime * smoothSpeed
            );
        }
        else
        {
            // návrat kamery do pùvodní pozice
            timer = 0f;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                Time.deltaTime * smoothSpeed
            );
        }
    }
}