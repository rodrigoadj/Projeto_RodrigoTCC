using TMPro;
using UnityEngine;

// HUD e painéis do nível (layout de referência: Doc_TCC/Interface_UI_Piffle, prancha HUD)
// mostra só informação funcional, igual nas duas versões; animações da versão B virão do FeedbackManager
public class HUD : MonoBehaviour
{
    [SerializeField] GerenciadorNivel gerenciador;
    [SerializeField] Ferramenta ferramenta;
    [SerializeField] MaoDoJogador mao;

    [Header("Gameplay")]
    [SerializeField] GameObject grupoGameplay;
    [SerializeField] TMP_Text textoMeta;
    [SerializeField] TMP_Text textoTempo;
    [SerializeField] TMP_Text textoPrompt;
    [SerializeField] TMP_Text textoMao;
    [SerializeField] TMP_Text textoFerramenta;
    [SerializeField] TMP_Text textoMensagem;
    [SerializeField] TMP_Text textoConcluir;
    [SerializeField] float duracaoMensagem = 2f;

    [Header("Briefing")]
    [SerializeField] GameObject painelBriefing;
    [SerializeField] TMP_Text briefingTitulo;
    [SerializeField] TMP_Text briefingInfo;
    [SerializeField] TMP_Text briefingDica;

    [Header("Resultado")]
    [SerializeField] GameObject painelResultado;
    [SerializeField] TMP_Text resultadoTitulo;
    [SerializeField] TMP_Text resultadoResumo;
    [SerializeField] TMP_Text resultadoBotao;

    float mensagemAte;

    void OnEnable()
    {
        gerenciador.AoMudarEstado += AtualizarEstado;
        gerenciador.AoMudarPontos += AtualizarMeta;
        gerenciador.AoMudarTempo += AtualizarTempo;
        ferramenta.AoMudarPrompt += AtualizarPrompt;
        mao.AoMudar += AtualizarMao;
        FeedbackManager.AoMostrarMensagem += MostrarMensagem;
    }

    void OnDisable()
    {
        gerenciador.AoMudarEstado -= AtualizarEstado;
        gerenciador.AoMudarPontos -= AtualizarMeta;
        gerenciador.AoMudarTempo -= AtualizarTempo;
        ferramenta.AoMudarPrompt -= AtualizarPrompt;
        mao.AoMudar -= AtualizarMao;
        FeedbackManager.AoMostrarMensagem -= MostrarMensagem;
    }

    void Start()
    {
        textoFerramenta.text = ferramenta.Dados != null ? ferramenta.Dados.nome : "";
        textoMensagem.text = "";
        textoPrompt.text = "";
        AtualizarMao(mao.Atual);
        AtualizarEstado();
    }

    void Update()
    {
        if (textoMensagem.text.Length > 0 && Time.unscaledTime >= mensagemAte)
            textoMensagem.text = "";
    }

    // botões (ligados no Inspector)
    public void BotaoComecar() => gerenciador.Comecar();
    public void BotaoContinuar() => gerenciador.Continuar();

    void AtualizarEstado()
    {
        EstadoNivel estado = gerenciador.Estado;
        grupoGameplay.SetActive(estado == EstadoNivel.Jogando);
        painelBriefing.SetActive(estado == EstadoNivel.Briefing);
        painelResultado.SetActive(estado == EstadoNivel.Resultado || estado == EstadoNivel.Fim);

        AtualizarMeta();
        AtualizarTempo();

        NivelData nivel = gerenciador.Atual;
        if (estado == EstadoNivel.Briefing)
        {
            briefingTitulo.text = $"Nível {nivel.numero}";
            briefingInfo.text = $"Meta: {nivel.meta} pontos de plástico no cesto vermelho\n"
                              + $"Tempo: {FormatarTempo(nivel.tempoSegundos)}\n"
                              + $"Obstáculos: {nivel.obstaculos}";
            briefingDica.text = nivel.dica;
        }
        else if (estado == EstadoNivel.Resultado)
        {
            bool vitoria = gerenciador.UltimoFoiVitoria;
            resultadoTitulo.text = vitoria ? $"Nível {nivel.numero} concluído!" : "Tempo esgotado";
            resultadoResumo.text = $"Pontos: {gerenciador.Pontos}/{nivel.meta}\n"
                                 + $"Separação correta: {gerenciador.Acertos}\n"
                                 + $"Cesto errado: {gerenciador.Erros}";
            resultadoBotao.text = vitoria ? "Continuar" : "Tentar de novo";
        }
        else if (estado == EstadoNivel.Fim)
        {
            resultadoTitulo.text = "Você entrou para a equipe!";
            resultadoResumo.text = "A Litorando na Limpeza aprovou seu trabalho.\nObrigado por jogar!";
            resultadoBotao.transform.parent.gameObject.SetActive(false);
        }
    }

    void AtualizarMeta()
    {
        textoMeta.text = $"Meta {gerenciador.Pontos}/{gerenciador.Atual.meta}";
        textoConcluir.gameObject.SetActive(gerenciador.MetaBatida && gerenciador.Estado == EstadoNivel.Jogando);
    }

    void AtualizarTempo()
    {
        textoTempo.text = FormatarTempo(gerenciador.TempoRestante);
    }

    void AtualizarPrompt(string prompt)
    {
        textoPrompt.text = prompt;
    }

    void AtualizarMao(ItemData item)
    {
        textoMao.text = item == null ? "Mão livre" : $"Na mão: {item.nome}";
    }

    void MostrarMensagem(string mensagem)
    {
        textoMensagem.text = mensagem;
        mensagemAte = Time.unscaledTime + duracaoMensagem;
    }

    static string FormatarTempo(float segundos)
    {
        int s = Mathf.CeilToInt(segundos);
        return $"{s / 60}:{s % 60:00}";
    }
}
