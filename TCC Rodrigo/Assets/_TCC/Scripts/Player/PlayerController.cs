using UnityEngine;
using UnityEngine.InputSystem;

// garante que o player sempre tem um Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference acaoMovimento;     // ação de input do teclado (WASD)
    [SerializeField] float velocidade = 5f;                  // velocidade de deslocamento

    Rigidbody rigidbodyComponente;                           // move o player pela física

    void Awake()
    {
        rigidbodyComponente = GetComponent<Rigidbody>();
        rigidbodyComponente.useGravity = true;                                   // gravidade ativa
        rigidbodyComponente.interpolation = RigidbodyInterpolation.Interpolate;  // movimento suave
        rigidbodyComponente.constraints = RigidbodyConstraints.FreezeRotation;   // não inclina ao colidir
    }

    void OnEnable()
    {
        if (acaoMovimento != null)
            acaoMovimento.action.Enable();
    }

    void OnDisable()
    {
        if (acaoMovimento != null)
            acaoMovimento.action.Disable();
    }

    void FixedUpdate()
    {

        Vector2 entrada = acaoMovimento.action.ReadValue<Vector2>();

        // move o player
        Vector3 direcao = transform.right * entrada.x + transform.forward * entrada.y;

        // evita ficar mais rápido andando na diagonal
        direcao = Vector3.ClampMagnitude(direcao, 1f);

        // mantém a gravidade e aplica a horizontal
        rigidbodyComponente.linearVelocity = new Vector3(direcao.x * velocidade, rigidbodyComponente.linearVelocity.y, direcao.z * velocidade);
    }
}