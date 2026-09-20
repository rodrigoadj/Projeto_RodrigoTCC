using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] InputActionReference acaoOlhar;        // ação de input do mouse (Inspector)
    [SerializeField] Transform transformCabeca;             // câmera/"cabeça" que inclina p/ cima e baixo
    [SerializeField] float sensibilidade = 0.1f;            // multiplicador do movimento do mouse
    [SerializeField] bool inverterY;                        // inverte o eixo vertical (tipo invert Y)
    [SerializeField] float minimoPitch = -90f;              // limite inferior da inclinação
    [SerializeField] float maximoPitch = 90f;               // limite superior da inclinação

    Rigidbody rigidbodyComponente;                          // gira o corpo inteiro do player
    float anguloHorizontal;                                 // yaw: giro horizontal (esquerda/direita)
    float anguloVertical;                                   // pitch: inclinação (cima/baixo)

    void Awake()
    {
        rigidbodyComponente = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // liga a ação de input quando o script é ativado
        if (acaoOlhar != null)
            acaoOlhar.action.Enable();
    }

    void OnDisable()
    {
        // desliga a ação de input quando o script é desativado
        if (acaoOlhar != null)
            acaoOlhar.action.Disable();
    }

    void Update()
    {

        Vector2 entradaMouse = acaoOlhar.action.ReadValue<Vector2>();


        anguloHorizontal += entradaMouse.x * sensibilidade;

        // acumula a inclinação da cabeça (respeitando o inverterY)
        float fatorInclinacao = inverterY ? -entradaMouse.y : entradaMouse.y;
        anguloVertical += fatorInclinacao * sensibilidade;

        // limite de inclinação
        anguloVertical = Mathf.Clamp(anguloVertical, minimoPitch, maximoPitch);
    }

    void FixedUpdate()
    {
        // gira o corpo inteiro na horizontal 
        if (rigidbodyComponente != null)
            rigidbodyComponente.MoveRotation(Quaternion.Euler(0f, anguloHorizontal, 0f));
    }

    void LateUpdate()
    {
        //  aplica só a inclinação na camera
        if (transformCabeca != null)
            transformCabeca.localRotation = Quaternion.Euler(anguloVertical, 0f, 0f);
    }
}