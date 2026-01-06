using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;

    public CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Input Actions")]
    public InputActionReference moveAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {

        // 1. Pobieramy referencjê do g³ównej kamery
        Transform cameraTransform = Camera.main.transform;

        // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // 2. Pobieramy wektory kierunkowe kamery (gdzie jest przód, gdzie prawo)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 3. Bardzo wa¿ne: "Sp³aszczamy" wektory (zerujemy oœ Y)
        // Dziêki temu, nawet jak kamera patrzy w dó³, postaæ idzie po p³askim terenie
        camForward.y = 0f;
        camRight.y = 0f;

        // 4. Normalizujemy wektory, aby ich d³ugoœæ wynosi³a 1 (¿eby ruch nie by³ szybszy po skosie)
        camForward.Normalize();
        camRight.Normalize();

        // 5. Obliczamy finalny kierunek ruchu
        // input.y (W/S) mno¿ymy przez przód kamery
        // input.x (A/D) mno¿ymy przez praw¹ stronê kamery
        Vector3 move = (camForward * input.y) + (camRight * input.x);

        // Reszta Twojego kodu pozostaje bez zmian...
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
        {
            // Opcjonalnie: dodaj Lerp (wyg³adzanie), ¿eby postaæ nie obraca³a siê natychmiastowo
            // transform.forward = move; // Stara wersja (natychmiastowa)

            // Nowa wersja (p³ynny obrót):
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

    }
}
