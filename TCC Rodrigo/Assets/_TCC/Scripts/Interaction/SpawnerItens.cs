using System.Collections.Generic;
using UnityEngine;

// gera os resíduos na praia no início de cada nível
// usa a seed do NivelData: o mesmo nível gera sempre a mesma praia (justo para as versões A e B)
public class SpawnerItens : MonoBehaviour
{
    [Header("Área de coleta (centrada neste objeto)")]
    [SerializeField] Vector2 tamanhoArea = new Vector2(20f, 20f);
    [SerializeField] float distanciaMinima = 1f;   // entre dois itens
    [SerializeField] Vector3[] pontosLivres;       // posições a evitar (ex.: cestos, início do jogador)
    [SerializeField] float raioLivre = 2f;

    [Header("Itens possíveis")]
    [SerializeField] List<ItemData> plasticosComuns = new List<ItemData>();
    [SerializeField] List<ItemData> plasticosRaros = new List<ItemData>();
    [SerializeField] List<ItemData> outrosResiduos = new List<ItemData>();

    readonly List<GameObject> gerados = new List<GameObject>();
    readonly List<Vector3> posicoes = new List<Vector3>();

    public IReadOnlyList<GameObject> Gerados => gerados;

    public void Gerar(NivelData nivel)
    {
        Limpar();
        var rng = new System.Random(nivel.seed);

        GerarCategoria(plasticosComuns, nivel.plasticosComuns, rng);
        GerarCategoria(plasticosRaros, nivel.plasticosRaros, rng);
        GerarCategoria(outrosResiduos, nivel.outrosResiduos, rng);
    }

    public void Limpar()
    {
        foreach (GameObject go in gerados)
        {
            if (go == null)
                continue;
            go.SetActive(false);   // Destroy só acontece no fim do frame; desativado, o raycast do novo spawn não acerta o item velho
            Destroy(go);
        }
        gerados.Clear();
        posicoes.Clear();
    }

    void GerarCategoria(List<ItemData> lista, int quantidade, System.Random rng)
    {
        if (lista.Count == 0)
            return;

        for (int i = 0; i < quantidade; i++)
        {
            ItemData item = lista[rng.Next(lista.Count)];
            Vector3 posicao = SortearPosicao(rng);
            float giro = (float)rng.NextDouble() * 360f;

            GameObject go = Instantiate(item.prefab, posicao, Quaternion.Euler(0f, giro, 0f), transform);
            go.name = item.nome;

            // apoia a base do modelo na areia (o pivô das primitivas fica no centro)
            Renderer render = go.GetComponentInChildren<Renderer>();
            if (render != null)
                go.transform.position += Vector3.up * (posicao.y - render.bounds.min.y);
            ItemColetavel coletavel = go.GetComponent<ItemColetavel>();
            if (coletavel == null)
                coletavel = go.AddComponent<ItemColetavel>();
            coletavel.Inicializar(item);

            gerados.Add(go);
            posicoes.Add(posicao);
        }
    }

    Vector3 SortearPosicao(System.Random rng)
    {
        Vector3 candidata = transform.position;
        for (int tentativa = 0; tentativa < 30; tentativa++)
        {
            float x = ((float)rng.NextDouble() - 0.5f) * tamanhoArea.x;
            float z = ((float)rng.NextDouble() - 0.5f) * tamanhoArea.y;
            candidata = transform.position + new Vector3(x, 0f, z);
            if (PosicaoLivre(candidata))
                break;
        }

        // apoia o item no chão
        if (Physics.Raycast(candidata + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, ~0, QueryTriggerInteraction.Ignore))
            candidata = hit.point;
        return candidata;
    }

    bool PosicaoLivre(Vector3 p)
    {
        foreach (Vector3 outra in posicoes)
            if (Vector3.Distance(p, outra) < distanciaMinima)
                return false;
        if (pontosLivres != null)
            foreach (Vector3 livre in pontosLivres)
                if (Vector2.Distance(new Vector2(p.x, p.z), new Vector2(livre.x, livre.z)) < raioLivre)
                    return false;
        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(tamanhoArea.x, 0.1f, tamanhoArea.y));
    }
}
