using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction;
    [SerializeField] float speed = 5f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void OnEnable() => moveAction?.action.Enable();
    void OnDisable() => moveAction?.action.Disable();

    void FixedUpdate()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 dir = transform.right * input.x + transform.forward * input.y;
        dir = Vector3.ClampMagnitude(dir, 1f);
        rb.linearVelocity = new Vector3(dir.x * speed, rb.linearVelocity.y, dir.z * speed);
    }
}