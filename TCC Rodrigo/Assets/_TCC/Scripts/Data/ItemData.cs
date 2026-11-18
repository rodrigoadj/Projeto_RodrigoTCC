using UnityEngine;

// tipos de plástico que valem pontos diferentes (GDD 2.2.3)
public enum TipoPlastico
{
    NaoEPlastico,
    Comum,          // 1 ponto
    Toxico,         // 2 pontos – PVC (3)
    NaoRecicla,     // 3 pontos – policarbonato (7)
    Microplastico   // 4 pontos – isopor/poliestireno (6)
}

// dados de um resíduo que aparece na praia
[CreateAssetMenu(fileName = "Item", menuName = "TCC/Item")]
public class ItemData : ScriptableObject
{
    public string nome;                          // aparece no prompt e na mão
    public TipoCesto cesto;                      // cesto correto (CONAMA 275)
    public TipoPlastico tipoPlastico;            // só para plásticos
    public int pontos;                           // pontos de meta (só plásticos somam)
    public GameObject prefab;                    // modelo na praia
    [TextArea] public string dicaEducativa;      // ex.: "Latinha é metal: cesto amarelo"

    public bool EhPlastico => cesto == TipoCesto.Plastico;
    public bool EhRaro => tipoPlastico == TipoPlastico.Toxico
                       || tipoPlastico == TipoPlastico.NaoRecicla
                       || tipoPlastico == TipoPlastico.Microplastico;
}
