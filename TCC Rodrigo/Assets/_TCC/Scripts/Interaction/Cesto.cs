using UnityEngine;

// cesto de coleta seletiva (posição fixa na praia)
public class Cesto : MonoBehaviour
{
    [SerializeField] TipoCesto tipo;

    public TipoCesto Tipo => tipo;

    public bool Aceita(ItemData item) => item != null && item.cesto == tipo;

    public static string Nome(TipoCesto tipo)
    {
        switch (tipo)
        {
            case TipoCesto.Plastico: return "Plástico";
            case TipoCesto.Metal:    return "Metal";
            case TipoCesto.Vidro:    return "Vidro";
            case TipoCesto.Papel:    return "Papel";
            case TipoCesto.Organico: return "Orgânico";
            default:                 return "";
        }
    }
}
