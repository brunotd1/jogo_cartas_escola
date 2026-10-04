using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Construtor e Gerenciador do Tabuleiro 4x5.
/// Cria as 4 linhas com 5 colunas cada (total 20 casas):
/// - Linha 0 (Inimigo - Fundo): Suporte e Distância
/// - Linha 1 (Inimigo - Frente): Corpo a Corpo
/// - Linha 2 (Jogador - Frente): Corpo a Corpo
/// - Linha 3 (Jogador - Fundo): Suporte e Distância
/// </summary>
public class Board : MonoBehaviour
{
    [Header("Estrutura do Tabuleiro (4x5)")]
    public const int TOTAL_ROWS = 4;
    public const int TOTAL_COLS = 5;

    [Tooltip("Matriz de slots acessível no código [linha, coluna]")]
    public BoardSlot[,] slots = new BoardSlot[TOTAL_ROWS, TOTAL_COLS];

    [Header("Dimensões Visuais dos Slots")]
    public float slotWidth = 140f;
    public float slotHeight = 180f;
    public float spacingX = 15f;
    public float spacingY = 15f;

    /// <summary>
    /// Configura ou localiza os slots do tabuleiro.
    /// </summary>
    void Awake()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        // Encontra todos os BoardSlots que são filhos deste tabuleiro
        BoardSlot[] foundSlots = GetComponentsInChildren<BoardSlot>();

        foreach (BoardSlot slot in foundSlots)
        {
            if (slot.row >= 0 && slot.row < TOTAL_ROWS && slot.col >= 0 && slot.col < TOTAL_COLS)
            {
                slots[slot.row, slot.col] = slot;
            }
        }
    }

    /// <summary>
    /// Método utilitário para gerar automaticamente o tabuleiro 4x5 com um clique no Inspector!
    /// </summary>
    [ContextMenu("Gerar Tabuleiro 4x5 Automaticamente")]
    public void GenerateBoardInEditor()
    {
        // Limpa filhos existentes
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // Calcula posições centrais
        float totalWidth = (TOTAL_COLS * slotWidth) + ((TOTAL_COLS - 1) * spacingX);
        float totalHeight = (TOTAL_ROWS * slotHeight) + ((TOTAL_ROWS - 1) * spacingY);

        float startX = -totalWidth / 2f + (slotWidth / 2f);
        float startY = totalHeight / 2f - (slotHeight / 2f);

        for (int r = 0; r < TOTAL_ROWS; r++)
        {
            // Define o lado e a tropa permitida para cada uma das 4 linhas:
            bool isPlayer = (r >= 2); // Linhas 2 e 3 são do jogador
            CardType allowedType = (r == 1 || r == 2) ? CardType.Melee : CardType.Ranged;

            string rowName = r switch
            {
                0 => "Linha_Inimigo_Distancia",
                1 => "Linha_Inimigo_CorpoACorpo",
                2 => "Linha_Jogador_CorpoACorpo",
                3 => "Linha_Jogador_Distancia",
                _ => $"Linha_{r}"
            };

            GameObject rowParent = new GameObject(rowName, typeof(RectTransform));
            rowParent.transform.SetParent(transform, false);

            for (int c = 0; c < TOTAL_COLS; c++)
            {
                float posX = startX + c * (slotWidth + spacingX);
                float posY = startY - r * (slotHeight + spacingY);

                GameObject slotObj = new GameObject($"Slot_[{r},{c}]", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(BoardSlot));
                slotObj.transform.SetParent(rowParent.transform, false);

                RectTransform rt = slotObj.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(slotWidth, slotHeight);
                rt.anchoredPosition = new Vector2(posX, posY);

                Image img = slotObj.GetComponent<Image>();
                // Cores sutis para diferenciar os lados:
                // Jogador: tom de azul-escuro medieval / Inimigo: tom de vermelho-escuro medieval
                if (isPlayer)
                {
                    img.color = (allowedType == CardType.Melee) 
                        ? new Color(0.15f, 0.25f, 0.35f, 0.6f) 
                        : new Color(0.12f, 0.20f, 0.30f, 0.6f);
                }
                else
                {
                    img.color = (allowedType == CardType.Melee) 
                        ? new Color(0.35f, 0.15f, 0.15f, 0.6f) 
                        : new Color(0.30f, 0.12f, 0.12f, 0.6f);
                }

                BoardSlot bs = slotObj.GetComponent<BoardSlot>();
                bs.row = r;
                bs.col = c;
                bs.isPlayerSide = isPlayer;
                bs.allowedCardType = allowedType;
                bs.slotImage = img;
                bs.normalColor = img.color;
            }
        }

        Debug.Log("Tabuleiro 4x5 medieval gerado com sucesso! (4 linhas x 5 colunas)");
    }
}
