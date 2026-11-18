using UnityEngine;

// dados de uma ferramenta de coleta (GDD 1.6 – Ferramentas diferentes)
[CreateAssetMenu(fileName = "Ferramenta", menuName = "TCC/Ferramenta")]
public class FerramentaData : ScriptableObject
{
    public string nome = "Apanhador Extralongo";
    public float alcance = 3f;                   // distância máxima da mira (m)
    [Min(1)] public int cliquesParaColetar = 2;  // "precisa acionar o botão do mouse várias vezes"
    public float multiplicadorVelocidade = 1f;   // a Peneira deixa o jogador mais lento (marco 4)
}
