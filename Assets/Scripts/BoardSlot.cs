using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Representa cada uma das 20 casas (slots) do tabuleiro 4x5.
/// Controla quem é o dono do slot (Jogador ou Inimigo), que tipo de tropa
/// pode ser colocada nele (Linha de Frente ou Linha de Trás) e qual carta está ocupando o espaço.
/// </summary>
public class BoardSlot : MonoBehaviour, IDropHandler
{
    [Header("Identificação no Tabuleiro")]
    [Tooltip("Índice da linha (0 a 3)")]
    public int row;

    [Tooltip("Índice da coluna (0 a 4)")]
    public int col;

    [Tooltip("Define se este slot pertence ao Jogador ou ao Oponente")]
    public bool isPlayerSide = true;

    [Tooltip("Define qual tipo de carta é permitida nesta linha")]
    public CardType allowedCardType = CardType.Melee;

    [Header("Estado do Slot")]
    [Tooltip("Carta que está atualmente posicionada nesta casa")]
    public CardDisplay currentCard = null;

    [Header("Visual do Slot")]
    public Image slotImage;
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    public Color highlightValidColor = new Color(0.2f, 0.8f, 0.2f, 0.6f);
    public Color highlightInvalidColor = new Color(0.8f, 0.2f, 0.2f, 0.6f);

    public bool IsOccupied => currentCard != null;

    void Awake()
    {
        if (slotImage == null)
            slotImage = GetComponent<Image>();

        SetHighlight(false);
    }

    /// <summary>
    /// Verifica se uma determinada carta tem permissão para ser jogada nesta casa:
    /// 1. A casa não pode estar ocupada.
    /// 2. A casa deve pertencer ao mesmo lado da carta (Jogador ou Oponente).
    /// 3. Linha de Frente só aceita Melee; Linha de Trás só aceita Ranged/Support.
    /// </summary>
    public bool CanPlaceCard(CardDisplay card)
    {
        if (card == null || IsOccupied) 
            return false;

        // Verifica se a carta pertence ao mesmo lado do slot
        if (card.isPlayerCard != isPlayerSide)
            return false;

        // Regra da Linha da Frente: apenas Corpo a Corpo
        if (allowedCardType == CardType.Melee)
        {
            return card.cardData.cardType == CardType.Melee;
        }

        // Regra da Linha de Trás: Ranged ou Support
        if (allowedCardType == CardType.Ranged || allowedCardType == CardType.Support)
        {
            return card.cardData.cardType == CardType.Ranged || card.cardData.cardType == CardType.Support;
        }

        return false;
    }

    /// <summary>
    /// Posiciona a carta nesta casa do tabuleiro.
    /// </summary>
    public void PlaceCard(CardDisplay card)
    {
        currentCard = card;
        card.transform.SetParent(transform);
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        Debug.Log($"Carta {card.cardData.cardName} posicionada no slot [{row}, {col}]!");
    }

    /// <summary>
    /// Remove a carta do slot (quando ela morre ou é movida).
    /// </summary>
    public void RemoveCard()
    {
        currentCard = null;
    }

    /// <summary>
    /// Altera o brilho da casa para indicar se o movimento é válido.
    /// </summary>
    public void SetHighlight(bool highlight, bool isValid = true)
    {
        if (slotImage == null) return;

        if (!highlight)
        {
            slotImage.color = normalColor;
        }
        else
        {
            slotImage.color = isValid ? highlightValidColor : highlightInvalidColor;
        }
    }

    /// <summary>
    /// Evento disparado quando o jogador solta (Drop) uma carta sobre este slot.
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        CardDragHandler dragHandler = eventData.pointerDrag.GetComponent<CardDragHandler>();
        if (dragHandler != null)
        {
            dragHandler.TryPlaceOnSlot(this);
        }
    }
}
