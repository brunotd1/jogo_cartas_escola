using UnityEngine;

/// <summary>
/// Define os tipos de tropas de acordo com a regra do jogo:
/// - Melee (Corpo a Corpo): Linha da frente, ataca cartas e herói, troca dano com outros Melee.
/// - Ranged (Ataque a Distância): Linha de trás, ataca apenas cartas inimigas.
/// - Support (Suporte): Linha de trás, concede suporte/buffs e ações estratégicas.
/// </summary>
public enum CardType
{
    Melee,
    Ranged,
    Support
}

/// <summary>
/// ScriptableObject que serve como "molde" ou "ficha" para qualquer carta do jogo.
/// Permite criar cartas facilmente pela interface da Unity sem escrever código repetido.
/// </summary>
[CreateAssetMenu(fileName = "NovaCarta", menuName = "Jogo de Cartas/Nova Carta")]
public class CardData : ScriptableObject
{
    [Header("Informações Básicas")]
    [Tooltip("Nome exibido na carta")]
    public string cardName = "Nova Carta";

    [TextArea(2, 4)]
    [Tooltip("Descrição de habilidades ou texto temático medieval")]
    public string description = "Descrição da tropa";

    [Tooltip("Ilustração visual da carta")]
    public Sprite cardArt;

    [Header("Tipo de Tropa e Posição")]
    [Tooltip("Define em qual linha do tabuleiro a carta deve ser colocada")]
    public CardType cardType = CardType.Melee;

    [Header("Estatísticas da Carta")]
    [Range(1, 10)]
    [Tooltip("Custo de mantimentos para invocar a carta (1 a 10)")]
    public int supplyCost = 1;

    [Min(0)]
    [Tooltip("Poder de ataque da carta")]
    public int attack = 1;

    [Min(1)]
    [Tooltip("Pontos de vida da carta")]
    public int maxHealth = 1;
}
