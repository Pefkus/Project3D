using UnityEngine;
using UnityEngine.InputSystem; // Wymagane dla nowego systemu

public class CameraRotate : MonoBehaviour
{
    [Header("Cel obserwacji")]
    public Transform target; // Obiekt, na który patrzymy

    [Header("Ustawienia")]
    public float distance = 5.0f;     // Odleg³oœæ od obiektu
    public float height = 2.0f;       // Sta³a wysokoœæ kamery wzglêdem obiektu
    public float sensitivity = 0.5f;  // Czu³oœæ obrotu

    private float currentAngleY = 0f; // Przechowuje aktualny k¹t obrotu

    void Start()
    {
        // Na starcie ustawiamy k¹t taki, jaki kamera ma aktualnie (¿eby nie przeskoczy³a)
        Vector3 angles = transform.eulerAngles;
        currentAngleY = angles.y;
    }

    // U¿ywamy LateUpdate, aby kamera porusza³a siê PO tym, jak obiekt siê ewentualnie poruszy³
    void LateUpdate()
    {
        if (target == null) return;

        // 1. Sprawdzamy czy Prawy Przycisk Myszy jest wciœniêty
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            // Ukrywamy kursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Pobieramy ruch myszy tylko w poziomie (oœ X myszy = obrót wokó³ osi Y œwiata)
            float mouseX = Mouse.current.delta.x.ReadValue();
            currentAngleY += mouseX * sensitivity;
        }
        else
        {
            // Pokazujemy kursor po puszczeniu przycisku
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 2. Obliczamy now¹ pozycjê
        // Tworzymy rotacjê tylko wokó³ osi Y
        Quaternion rotation = Quaternion.Euler(0, currentAngleY, 0);

        // Pozycja to: Pozycja celu - (wektor w ty³ * dystans) + wysokoœæ
        // Mno¿enie rotation * Vector3.forward obraca wektor kierunkowy
        Vector3 positionOffset = rotation * Vector3.back * distance;

        // Ustawiamy finaln¹ pozycjê kamery
        transform.position = target.position + positionOffset + new Vector3(0, height, 0);

        // 3. Wymuszamy patrzenie na cel (dla pewnoœci)
        // Patrzymy na œrodek obiektu + ewentualnie lekkie przesuniêcie w górê, ¿eby nie patrzeæ w stopy
        transform.LookAt(target.position + new Vector3(0, height * 0.5f, 0));
    }
}
