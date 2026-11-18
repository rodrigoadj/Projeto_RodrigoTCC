using System.Collections.Generic;
using UnityEngine;

// resíduo que pode ser coletado com a ferramenta
public class ItemColetavel : MonoBehaviour
{
    [SerializeField] ItemData dados;
    [SerializeField] Material materialSilhueta;  // contorno quando mirado (feedback funcional, nas duas versões)

    Renderer[] renderers;
    bool destacado;

    public ItemData Dados => dados;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void Inicializar(ItemData itemData)
    {
        dados = itemData;
    }

    // liga/desliga a silhueta adicionando o material como material extra do renderer
    public void Destacar(bool ligar)
    {
        if (ligar == destacado || materialSilhueta == null)
            return;
        destacado = ligar;

        var materiais = new List<Material>();
        foreach (Renderer r in renderers)
        {
            r.GetSharedMaterials(materiais);
            if (ligar)
                materiais.Add(materialSilhueta);
            else
                materiais.Remove(materialSilhueta);
            r.SetSharedMaterials(materiais);
        }
    }
}
