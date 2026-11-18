using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// gera as duas builds do experimento a partir do mesmo projeto
// A = feedback básico (define TCC_FEEDBACK_BASICO) | B = feedback elaborado
// linha de comando: unity build "<projeto>" --target StandaloneWindows64 --execute-method BuildVersoes.BuildAmbas
public static class BuildVersoes
{
    const string PastaBuilds = "Builds";
    const string NomeExe = "LimpandoAPraia.exe";
    const string DefineBasico = "TCC_FEEDBACK_BASICO";

    [MenuItem("TCC/Build/A - Básico")]
    public static void BuildBasico() => Gerar(FeedbackMode.Basico);

    [MenuItem("TCC/Build/B - Elaborado")]
    public static void BuildElaborado() => Gerar(FeedbackMode.Elaborado);

    [MenuItem("TCC/Build/Ambas (A e B)")]
    public static void BuildAmbas()
    {
        bool ok = Gerar(FeedbackMode.Basico) && Gerar(FeedbackMode.Elaborado);
        if (Application.isBatchMode)
            EditorApplication.Exit(ok ? 0 : 1);
    }

    static bool Gerar(FeedbackMode modo)
    {
        string nomePasta = modo == FeedbackMode.Basico ? "A_Basico" : "B_Elaborado";
        string pasta = Path.Combine(PastaBuilds, nomePasta);

        if (Directory.Exists(pasta))
            Directory.Delete(pasta, true);

        var opcoes = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = Path.Combine(pasta, NomeExe),
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
            extraScriptingDefines = modo == FeedbackMode.Basico ? new[] { DefineBasico } : new string[0],
            options = BuildOptions.None
        };

        BuildReport relatorio = BuildPipeline.BuildPlayer(opcoes);
        if (relatorio.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError($"[TCC] Build {nomePasta} falhou: {relatorio.summary.result}");
            return false;
        }

        // pastas de debug que a Unity manda não distribuir
        foreach (string lixo in Directory.GetDirectories(pasta, "*DoNotShip"))
            Directory.Delete(lixo, true);

        // zip para distribuir aos participantes (GDD 9.2: baixar o zip e extrair)
        string zip = Path.Combine(PastaBuilds, $"LimpandoAPraia_{nomePasta}.zip");
        if (File.Exists(zip))
            File.Delete(zip);
        ZipFile.CreateFromDirectory(pasta, zip);

        Debug.Log($"[TCC] Build {nomePasta} OK: {Path.GetFullPath(zip)}");
        return true;
    }
}
