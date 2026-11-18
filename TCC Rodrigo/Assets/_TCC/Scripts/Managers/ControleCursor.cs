using UnityEngine;

// trava o cursor e libera o controle do jogador só durante o estado Jogando
public class ControleCursor : MonoBehaviour
{
    [SerializeField] GerenciadorNivel gerenciador;
    [SerializeField] PlayerController movimento;
    [SerializeField] MouseLook olhar;

    void OnEnable()
    {
        gerenciador.AoMudarEstado += Aplicar;
    }

    void OnDisable()
    {
        gerenciador.AoMudarEstado -= Aplicar;
    }

    void Start()
    {
        Aplicar();
    }

    void Aplicar()
    {
        bool jogando = gerenciador.Estado == EstadoNivel.Jogando;

        Cursor.lockState = jogando ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !jogando;

        if (movimento != null)
        {
            movimento.enabled = jogando;
            Rigidbody rb = movimento.GetComponent<Rigidbody>();
            if (!jogando && rb != null)
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);   // não desliza parado
        }
        if (olhar != null)
            olhar.enabled = jogando;
    }
}
