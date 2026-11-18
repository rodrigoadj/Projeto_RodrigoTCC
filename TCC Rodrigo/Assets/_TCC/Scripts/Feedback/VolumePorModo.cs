using UnityEngine;
using UnityEngine.Rendering;

// escolhe o perfil de pós-processamento conforme a versão do jogo
// básico: só tonemapping | elaborado: bloom, color grading de pôr do sol e vinheta (GDD 11.3)
[RequireComponent(typeof(Volume))]
public class VolumePorModo : MonoBehaviour
{
    [SerializeField] VolumeProfile perfilBasico;
    [SerializeField] VolumeProfile perfilElaborado;

    void Awake()
    {
        GetComponent<Volume>().sharedProfile = ModoFeedback.Elaborado ? perfilElaborado : perfilBasico;
    }
}
