using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Uzamkne a skryje kurzor myši uprostøed obrazovky
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Zabrání pøetoèení hlavy dozadu

        // Rotace kamery nahoru/dolù
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotace celého tìla hráèe doleva/doprava
        playerBody.Rotate(Vector3.up * mouseX);
    }
}