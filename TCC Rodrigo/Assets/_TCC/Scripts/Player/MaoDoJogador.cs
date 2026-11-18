using System;
using UnityEngine;

// resíduo que o jogador está segurando (só 1 por vez)
public class MaoDoJogador : MonoBehaviour
{
    public event Action<ItemData> AoMudar;       // HUD: "Mão livre" ou nome do item

    ItemColetavel itemSegurado;

    public bool Livre => itemSegurado == null;
    public ItemData Atual => itemSegurado != null ? itemSegurado.Dados : null;

    public void Segurar(ItemColetavel item)
    {
        itemSegurado = item;
        item.Destacar(false);
        item.gameObject.SetActive(false);        // sai do cenário no mesmo frame (nas duas versões)
        AoMudar?.Invoke(Atual);
    }

    // depositou no cesto certo: o item sai do jogo
    public void Descartar()
    {
        if (itemSegurado != null)
            Destroy(itemSegurado.gameObject);
        itemSegurado = null;
        AoMudar?.Invoke(null);
    }

    // início de nível: mão vazia
    public void Limpar()
    {
        itemSegurado = null;
        AoMudar?.Invoke(null);
    }
}
