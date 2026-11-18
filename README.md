# Projeto_RodrigoTCC

**Limpando a Praia** é um jogo sério em primeira pessoa sobre coleta seletiva e plástico nas praias, feito em Unity 6000.3.8f1 com URP. O GDD está em `Doc_TCC/`.

## Versões do experimento (GDD – Seção XI)

O TCC compara duas versões do mesmo jogo:

| Build | Versão | Feedback |
|---|---|---|
| **A** | Protótipo | Básico: HUD numérico, textos simples, música ambiente e só tonemapping |
| **B** | Final | Elaborado: SFX, partículas, animações de UI, bloom, color grading e vinheta |

O feedback é a **variável independente**. Regras, níveis, spawn, tempo e controles são idênticos nas duas builds, que saem da mesma base de código.

### Regras para o código
1. A gameplay **só avisa o evento**: `FeedbackManager.Notificar(FeedbackEvento.DepositoCorreto, contexto)`. Nenhum script de gameplay toca som ou cria efeito diretamente.
2. Feedback **nunca muda o estado nem o tempo do jogo**: nada de hit-stop nem de `Time.timeScale`. Animações como "item voa até a mão" são só visuais.
3. O que muda em cada versão está no `switch` do `FeedbackManager`, que segue a ordem da tabela 11.3 do GDD.

### Arquivos principais (`TCC Rodrigo/Assets/_TCC/`)
- `Scripts/Feedback/ModoFeedback.cs`: indica qual versão está rodando.
- `Scripts/Feedback/FeedbackManager.cs`: ponto único de feedback.
- `Scripts/Feedback/FeedbackConfig.cs`: sons e partículas da versão elaborada.
- `Scripts/Feedback/VolumePorModo.cs`: aplica `Settings/PP_Basico` ou `Settings/PP_Elaborado` ao Global Volume.

### Testar no Editor
Escolha **TCC › Modo de Feedback (Editor) › A - Básico / B - Elaborado** e dê Play. O console mostra `[TCC] FeedbackMode=...`.

### Gerar as builds
- **Pelo Editor:** use **TCC › Build › Ambas (A e B)**. Também há opções para gerar só A ou só B.
- **Pela linha de comando:** com o Editor fechado, rode:
  ```
  unity build "TCC Rodrigo" --target StandaloneWindows64 --execute-method BuildVersoes.BuildAmbas
  ```

A build A recebe o define `TCC_FEEDBACK_BASICO`. A saída fica em `TCC Rodrigo/Builds/`, com `A_Basico/`, `B_Elaborado/` e um `.zip` de cada para distribuir.
