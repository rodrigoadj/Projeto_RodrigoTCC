using UnityEngine;

// referências usadas SOMENTE pela versão elaborada (sons, partículas, tempos de animação)
// na versão básica estas referências simplesmente não são usadas
[CreateAssetMenu(fileName = "FeedbackConfig", menuName = "TCC/Feedback Config")]
public class FeedbackConfig : ScriptableObject
{
    [Header("Sons")]
    public AudioClip somColetar;
    public AudioClip somItemEscondido;
    public AudioClip somDepositoCorreto;
    public AudioClip somDepositoErrado;
    public AudioClip somAlerta30s;
    public AudioClip jingleMetaAtingida;
    public AudioClip somChute;
    public AudioClip somLixoNoMar;
    public AudioClip somMoedas;
    public AudioClip somMelhoria;

    [Header("Partículas")]
    public GameObject particulaAreia;
    public GameObject particulaCesto;
    public GameObject confete;

    [Header("Animações de UI (GDD 5.2: 0,15–0,3 s)")]
    [Range(0.15f, 0.3f)] public float duracaoAnimacaoUI = 0.2f;
}
