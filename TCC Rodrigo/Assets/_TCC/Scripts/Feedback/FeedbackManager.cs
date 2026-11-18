using System;
using UnityEngine;

// ponto ÚNICO de feedback do jogo (GDD 11.4)
//
// REGRAS para manter a validade do experimento:
// 1. a gameplay só avisa o evento: FeedbackManager.Notificar(FeedbackEvento.X, contexto)
//    nenhum script de gameplay toca som ou cria efeito diretamente
// 2. feedback NUNCA muda o estado nem o tempo do jogo (sem hit-stop, sem Time.timeScale)
//    ex.: "item voa até a mão" é só visual; o item já está na mão no mesmo frame nas duas versões
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instancia { get; private set; }

    // resposta comum às duas versões: mensagem de texto simples (o HUD se inscreve aqui)
    public static event Action<string> AoMostrarMensagem;

    [SerializeField] FeedbackConfig config;   // só usado na versão elaborada
    [SerializeField] AudioSource fonteSFX;    // só usado na versão elaborada

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    void OnDestroy()
    {
        if (Instancia == this)
            Instancia = null;
    }

    // chamado pela gameplay
    public static void Notificar(FeedbackEvento evento, FeedbackContexto contexto = default)
    {
        // coluna "Versão básica" da tabela 11.3 — roda nas DUAS versões
        string mensagem = MensagemBasica(evento, contexto);
        if (mensagem != null)
            AoMostrarMensagem?.Invoke(mensagem);

        // coluna "Versão elaborada" — extras que só existem na build B
        if (ModoFeedback.Elaborado && Instancia != null)
            Instancia.FeedbackElaborado(evento, contexto);
    }

    // textos simples da versão básica (também aparecem na elaborada)
    public static string MensagemBasica(FeedbackEvento evento, FeedbackContexto contexto)
    {
        switch (evento)
        {
            case FeedbackEvento.ItemEscondidoEncontrado: return "Item encontrado";
            case FeedbackEvento.DepositoErrado:          return "Cesto errado";
            case FeedbackEvento.MetaAtingida:            return "Meta atingida";
            case FeedbackEvento.Vitoria:                 return "Vitória!";
            case FeedbackEvento.Derrota:                 return "Tempo esgotado";
            case FeedbackEvento.MelhoriaEscolhida:       return $"Melhoria escolhida: {contexto.texto}";
            default:                                     return null; // só o HUD numérico atualiza
        }
    }

    // extras da versão elaborada, na mesma ordem da tabela 11.3
    // os itens marcados com TODO dependem de assets/UI que ainda não existem (marco 6)
    void FeedbackElaborado(FeedbackEvento evento, FeedbackContexto contexto)
    {
        switch (evento)
        {
            case FeedbackEvento.MirarColetavel:
                // TODO: silhueta com leve pulsação + ícone da ferramenta
                break;
            case FeedbackEvento.ColetarItem:
                Tocar(config ? config.somColetar : null);
                Particula(config ? config.particulaAreia : null, contexto.posicao);
                // TODO: item "voa" até a mão (só visual)
                break;
            case FeedbackEvento.ItemEscondidoEncontrado:
                Tocar(config ? config.somItemEscondido : null);
                // TODO: brilho + ícone do material
                break;
            case FeedbackEvento.DepositoCorreto:
                Tocar(config ? config.somDepositoCorreto : null);
                Particula(config ? config.particulaCesto : null, contexto.posicao);
                // TODO: texto da meta balança + "+N" flutuante
                break;
            case FeedbackEvento.DepositoErrado:
                Tocar(config ? config.somDepositoErrado : null);
                // TODO: cesto treme + dica educativa curta sobre o material
                break;
            case FeedbackEvento.Faltam30s:
                Tocar(config ? config.somAlerta30s : null);
                // TODO: temporizador pisca em vermelho
                break;
            case FeedbackEvento.MetaAtingida:
                Tocar(config ? config.jingleMetaAtingida : null);
                Particula(config ? config.confete : null, contexto.posicao);
                // TODO: botão "Concluir nível" em destaque
                break;
            case FeedbackEvento.NpcChutaLixo:
                Tocar(config ? config.somChute : null);
                // TODO: ícone de alerta sobre o item
                break;
            case FeedbackEvento.VentaniaInicio:
            case FeedbackEvento.OndaInicio:
                // TODO: som ambiente, partículas de vento/espuma, aviso de direção
                break;
            case FeedbackEvento.LixoLevadoAoMar:
                Tocar(config ? config.somLixoNoMar : null);
                // TODO: texto educativo "Esse plástico foi para o oceano"
                break;
            case FeedbackEvento.Vitoria:
            case FeedbackEvento.Derrota:
                // TODO: música de vitória/derrota, UI animada, contadores animados
                break;
            case FeedbackEvento.Compra:
            case FeedbackEvento.Venda:
                Tocar(config ? config.somMoedas : null);
                // TODO: contador de moedas animado
                break;
            case FeedbackEvento.MelhoriaEscolhida:
                Tocar(config ? config.somMelhoria : null);
                // TODO: área limite se expande visualmente / relógio animado
                break;
        }
    }

    void Tocar(AudioClip clip)
    {
        if (clip != null && fonteSFX != null)
            fonteSFX.PlayOneShot(clip);
    }

    void Particula(GameObject prefab, Vector3 posicao)
    {
        if (prefab != null)
            Instantiate(prefab, posicao, Quaternion.identity);
    }
}
