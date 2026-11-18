using UnityEngine;

// configuração de um nível (GDD 1.7.2)
[CreateAssetMenu(fileName = "Nivel", menuName = "TCC/Nível")]
public class NivelData : ScriptableObject
{
    public int numero = 1;
    public int meta = 10;                        // pontos de plástico depositados no cesto vermelho
    public float tempoSegundos = 60f;

    [Header("Spawn")]
    [Tooltip("Seed fixa: todos os jogadores (versão A e B) recebem a mesma praia neste nível")]
    public int seed = 101;
    public int plasticosComuns = 12;
    public int plasticosRaros = 2;
    public int outrosResiduos = 5;               // metal, vidro, papel e orgânico (não somam meta)

    [Header("Briefing")]
    public string obstaculos = "Pessoas andando";
    [TextArea] public string dica;
}
