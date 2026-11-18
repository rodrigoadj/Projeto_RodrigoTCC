using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ferramenta de coleta: mira pelo centro da tela, coleta itens e deposita nos cestos
public class Ferramenta : MonoBehaviour
{
    [SerializeField] FerramentaData dados;
    [SerializeField] Camera cameraJogador;
    [SerializeField] MaoDoJogador mao;
    [SerializeField] GerenciadorNivel gerenciador;
    [SerializeField] InputActionReference acaoUsar;   // botão esquerdo do mouse (Player/Attack)

    public event Action<string> AoMudarPrompt;        // texto do centro da tela

    ItemColetavel itemMirado;
    Cesto cestoMirado;
    int cliques;
    string ultimoPrompt;

    public FerramentaData Dados => dados;
    public int Cliques => cliques;

    void OnEnable()
    {
        if (acaoUsar != null)
            acaoUsar.action.Enable();
    }

    void OnDisable()
    {
        if (acaoUsar != null)
            acaoUsar.action.Disable();
    }

    void Update()
    {
        if (gerenciador == null || gerenciador.Estado != EstadoNivel.Jogando)
        {
            Mirar(null, null);
            AtualizarPrompt();
            return;
        }

        AtualizarMira();
        if (acaoUsar != null && acaoUsar.action.WasPressedThisFrame())
            Usar();
        AtualizarPrompt();
    }

    void AtualizarMira()
    {
        Ray raio = cameraJogador.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ItemColetavel item = null;
        Cesto cesto = null;
        if (Physics.Raycast(raio, out RaycastHit hit, dados.alcance, ~0, QueryTriggerInteraction.Ignore))
        {
            item = hit.collider.GetComponentInParent<ItemColetavel>();
            cesto = hit.collider.GetComponentInParent<Cesto>();
        }
        Mirar(item, cesto);
    }

    // troca o alvo da mira (público para testes)
    public void Mirar(ItemColetavel item, Cesto cesto)
    {
        cestoMirado = cesto;
        if (item == itemMirado)
            return;

        if (itemMirado != null)
            itemMirado.Destacar(false);
        itemMirado = item;
        cliques = 0;

        if (itemMirado != null)
        {
            itemMirado.Destacar(true);
            FeedbackManager.Notificar(FeedbackEvento.MirarColetavel, Contexto(itemMirado.Dados, itemMirado.transform));
        }
    }

    // um clique da ferramenta
    public void Usar()
    {
        if (itemMirado != null && mao.Livre)
        {
            cliques++;
            if (cliques >= dados.cliquesParaColetar)
                Coletar(itemMirado);
        }
        else if (cestoMirado != null && !mao.Livre)
        {
            Depositar(cestoMirado);
        }
    }

    void Coletar(ItemColetavel item)
    {
        Vector3 posicao = item.transform.position;
        itemMirado = null;
        cliques = 0;
        mao.Segurar(item);
        FeedbackContexto ctx = Contexto(item.Dados, null);
        ctx.posicao = posicao;
        FeedbackManager.Notificar(FeedbackEvento.ColetarItem, ctx);
    }

    void Depositar(Cesto cesto)
    {
        ItemData item = mao.Atual;
        bool correto = cesto.Aceita(item);
        FeedbackContexto ctx = Contexto(item, cesto.transform);
        ctx.posicao = cesto.transform.position + Vector3.up;

        if (correto)
        {
            mao.Descartar();
            gerenciador.RegistrarDeposito(item, true);
            FeedbackManager.Notificar(FeedbackEvento.DepositoCorreto, ctx);
        }
        else
        {
            // item rejeitado: continua na mão; a penalidade é só o tempo perdido
            gerenciador.RegistrarDeposito(item, false);
            FeedbackManager.Notificar(FeedbackEvento.DepositoErrado, ctx);
        }
    }

    static FeedbackContexto Contexto(ItemData item, Transform alvo)
    {
        return new FeedbackContexto
        {
            posicao = alvo != null ? alvo.position : Vector3.zero,
            alvo = alvo,
            material = item != null ? item.cesto : TipoCesto.Nenhum,
            valor = item != null && item.EhPlastico ? item.pontos : 0,
            texto = item != null ? item.dicaEducativa : null
        };
    }

    void AtualizarPrompt()
    {
        string prompt = "";
        if (gerenciador != null && gerenciador.Estado == EstadoNivel.Jogando)
        {
            if (itemMirado != null)
            {
                if (!mao.Livre)
                    prompt = "Mão ocupada · deposite no cesto";
                else if (dados.cliquesParaColetar > 1)
                    prompt = $"Clique · Coletar {itemMirado.Dados.nome} ({cliques}/{dados.cliquesParaColetar})";
                else
                    prompt = $"Clique · Coletar {itemMirado.Dados.nome}";
            }
            else if (cestoMirado != null)
            {
                prompt = mao.Livre
                    ? $"Cesto · {Cesto.Nome(cestoMirado.Tipo)}"
                    : $"Clique · Depositar em {Cesto.Nome(cestoMirado.Tipo)}";
            }
        }

        if (prompt != ultimoPrompt)
        {
            ultimoPrompt = prompt;
            AoMudarPrompt?.Invoke(prompt);
        }
    }
}
