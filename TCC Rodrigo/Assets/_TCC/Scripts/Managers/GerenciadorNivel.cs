using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EstadoNivel
{
    Briefing,   // painel antes do nível, aguardando "Começar"
    Jogando,    // temporizador correndo
    Resultado,  // vitória ou derrota
    Fim         // terminou o nível 3
}

// ciclo do nível: Briefing → Jogando → Resultado → próximo nível (GDD 1.4)
public class GerenciadorNivel : MonoBehaviour
{
    [SerializeField] NivelData[] niveis;
    [SerializeField] SpawnerItens spawner;
    [SerializeField] MaoDoJogador mao;
    [SerializeField] Transform jogador;
    [SerializeField] Transform pontoInicial;

    public event Action AoMudarEstado;
    public event Action AoMudarPontos;
    public event Action AoMudarTempo;

    public EstadoNivel Estado { get; private set; }
    public NivelData Atual => niveis[IndiceNivel];
    public int IndiceNivel { get; private set; }
    public int TotalNiveis => niveis.Length;
    public int Pontos { get; private set; }
    public float TempoRestante { get; private set; }
    public bool MetaBatida { get; private set; }
    public bool UltimoFoiVitoria { get; private set; }

    // estatísticas do nível (tela de Resultados e telemetria)
    public int Acertos { get; private set; }
    public int Erros { get; private set; }

    bool avisou30s;

    void Start()
    {
        IrParaBriefing(0);
    }

    void Update()
    {
        if (Estado != EstadoNivel.Jogando)
            return;

        DefinirTempo(TempoRestante - Time.deltaTime);

        Keyboard teclado = Keyboard.current;
        if (MetaBatida && teclado != null && (teclado.enterKey.wasPressedThisFrame || teclado.numpadEnterKey.wasPressedThisFrame))
            ConcluirNivel();
    }

    public void IrParaBriefing(int indice)
    {
        IndiceNivel = Mathf.Clamp(indice, 0, niveis.Length - 1);
        Pontos = 0;
        Acertos = 0;
        Erros = 0;
        MetaBatida = false;
        avisou30s = false;
        TempoRestante = Atual.tempoSegundos;

        mao.Limpar();
        spawner.Gerar(Atual);
        ReposicionarJogador();

        AoMudarPontos?.Invoke();
        AoMudarTempo?.Invoke();
        MudarEstado(EstadoNivel.Briefing);
    }

    // botão "Começar" do briefing
    public void Comecar()
    {
        if (Estado == EstadoNivel.Briefing)
            MudarEstado(EstadoNivel.Jogando);
    }

    // botão do painel de resultado
    public void Continuar()
    {
        if (Estado != EstadoNivel.Resultado)
            return;

        if (!UltimoFoiVitoria)
            IrParaBriefing(IndiceNivel);            // derrota: reinicia o nível
        else if (IndiceNivel + 1 < niveis.Length)
            IrParaBriefing(IndiceNivel + 1);
        else
            MudarEstado(EstadoNivel.Fim);
    }

    // "[Enter] Concluir nível" depois de bater a meta
    public void ConcluirNivel()
    {
        if (Estado == EstadoNivel.Jogando && MetaBatida)
            Terminar(true);
    }

    public void RegistrarDeposito(ItemData item, bool correto)
    {
        if (!correto)
        {
            Erros++;
            return;
        }

        Acertos++;
        if (!item.EhPlastico)
            return;                                 // só plástico soma meta

        Pontos += item.pontos;
        AoMudarPontos?.Invoke();

        if (!MetaBatida && Pontos >= Atual.meta)
        {
            MetaBatida = true;
            FeedbackManager.Notificar(FeedbackEvento.MetaAtingida, FeedbackContexto.Em(jogador != null ? jogador.position : Vector3.zero));
        }
    }

    // público para testes (ex.: pular para 31 s)
    public void DefinirTempo(float segundos)
    {
        float anterior = TempoRestante;
        TempoRestante = Mathf.Max(0f, segundos);
        AoMudarTempo?.Invoke();

        if (!avisou30s && anterior > 30f && TempoRestante <= 30f)
        {
            avisou30s = true;
            FeedbackManager.Notificar(FeedbackEvento.Faltam30s);
        }

        if (Estado == EstadoNivel.Jogando && TempoRestante <= 0f)
            Terminar(MetaBatida);                  // tempo acabou: vence quem já bateu a meta
    }

    void Terminar(bool vitoria)
    {
        UltimoFoiVitoria = vitoria;
        MudarEstado(EstadoNivel.Resultado);
        FeedbackContexto ctx = new FeedbackContexto { valor = Pontos };
        FeedbackManager.Notificar(vitoria ? FeedbackEvento.Vitoria : FeedbackEvento.Derrota, ctx);
    }

    void MudarEstado(EstadoNivel novo)
    {
        Estado = novo;
        Debug.Log($"[TCC] Nível {Atual.numero} · {novo} · pontos={Pontos}/{Atual.meta} · acertos={Acertos} erros={Erros} · tempo={TempoRestante:0.0}");
        AoMudarEstado?.Invoke();
    }

    void ReposicionarJogador()
    {
        if (jogador == null || pontoInicial == null)
            return;
        Rigidbody rb = jogador.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = pontoInicial.position;
        }
        jogador.SetPositionAndRotation(pontoInicial.position, pontoInicial.rotation);
    }
}
