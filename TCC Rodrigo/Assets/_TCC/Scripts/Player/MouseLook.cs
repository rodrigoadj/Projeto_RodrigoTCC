using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] InputActionReference lookAction;
    [SerializeField] Transform headTransform;
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] bool invertY;
    [SerializeField] float minPitch = -90f, maxPitch = 90f;

    Rigidbody rb;
    float yaw, pitch;

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable() => lookAction?.action.Enable();
    void OnDisable() => lookAction?.action.Disable();

    void Update()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();
        yaw   += look.x * sensitivity;
        pitch += (invertY ? -look.y : look.y) * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void FixedUpdate()
    {
        if (rb)
            rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }

    void LateUpdate()
    {
        if (headTransform)
            headTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}