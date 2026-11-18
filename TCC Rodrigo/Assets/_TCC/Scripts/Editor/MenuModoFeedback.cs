using UnityEditor;

// alterna a versão usada no Play Mode do Editor, sem recompilar
// (as builds usam o define colocado pelo BuildVersoes, não este menu)
public static class MenuModoFeedback
{
    const string MenuBasico = "TCC/Modo de Feedback (Editor)/A - Básico";
    const string MenuElaborado = "TCC/Modo de Feedback (Editor)/B - Elaborado";

    [MenuItem(MenuBasico)]
    static void UsarBasico() => ModoFeedback.Atual = FeedbackMode.Basico;

    [MenuItem(MenuElaborado)]
    static void UsarElaborado() => ModoFeedback.Atual = FeedbackMode.Elaborado;

    [MenuItem(MenuBasico, true)]
    static bool ValidarBasico()
    {
        Menu.SetChecked(MenuBasico, ModoFeedback.Atual == FeedbackMode.Basico);
        return true;
    }

    [MenuItem(MenuElaborado, true)]
    static bool ValidarElaborado()
    {
        Menu.SetChecked(MenuElaborado, ModoFeedback.Atual == FeedbackMode.Elaborado);
        return true;
    }
}
