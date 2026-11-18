using UnityEngine;

// as duas versões do experimento do TCC (GDD, Seção XI)
public enum FeedbackMode
{
    Basico,     // build A – protótipo, feedback básico
    Elaborado   // build B – versão final, feedback visual e sonoro elaborado
}

// diz qual versão do jogo está rodando
// - na build: definido pelo define TCC_FEEDBACK_BASICO, colocado pelo BuildVersoes
// - no Editor: definido pelo menu TCC > Modo de Feedback (EditorPrefs), sem recompilar
public static class ModoFeedback
{
#if UNITY_EDITOR
    public const string ChaveEditorPrefs = "TCC.FeedbackMode";

    public static FeedbackMode Atual
    {
        get => (FeedbackMode)UnityEditor.EditorPrefs.GetInt(ChaveEditorPrefs, (int)FeedbackMode.Elaborado);
        set => UnityEditor.EditorPrefs.SetInt(ChaveEditorPrefs, (int)value);
    }
#elif TCC_FEEDBACK_BASICO
    public static FeedbackMode Atual => FeedbackMode.Basico;
#else
    public static FeedbackMode Atual => FeedbackMode.Elaborado;
#endif

    public static bool Elaborado => Atual == FeedbackMode.Elaborado;

    // registra a versão no Player.log (útil para conferir a build e para a telemetria)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RegistrarNoLog()
    {
        Debug.Log($"[TCC] FeedbackMode={Atual}");
    }
}
