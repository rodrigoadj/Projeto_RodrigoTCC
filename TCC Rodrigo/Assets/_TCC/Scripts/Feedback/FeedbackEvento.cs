using UnityEngine;

// um evento para cada linha da tabela 11.3 do GDD (básico × elaborado)
public enum FeedbackEvento
{
    MirarColetavel,
    ColetarItem,
    ItemEscondidoEncontrado,
    DepositoCorreto,
    DepositoErrado,
    Faltam30s,
    MetaAtingida,
    NpcChutaLixo,
    VentaniaInicio,
    OndaInicio,
    LixoLevadoAoMar,
    Vitoria,
    Derrota,
    Compra,
    Venda,
    MelhoriaEscolhida
}

// tipos de cesto da coleta seletiva (CONAMA 275/2001)
public enum TipoCesto
{
    Nenhum,
    Plastico,   // vermelho
    Metal,      // amarelo
    Vidro,      // verde
    Papel,      // azul
    Organico    // marrom
}

// dados que a gameplay manda junto com o evento
public struct FeedbackContexto
{
    public Vector3 posicao;      // onde aconteceu (partículas, "+N" flutuante)
    public Transform alvo;       // objeto envolvido (item, cesto, NPC)
    public TipoCesto material;   // material do item / cor do cesto
    public int valor;            // pontos (+N) ou moedas
    public string texto;         // texto extra (ex.: nome da melhoria, dica educativa)

    public static FeedbackContexto Em(Vector3 posicao) => new FeedbackContexto { posicao = posicao };
}
