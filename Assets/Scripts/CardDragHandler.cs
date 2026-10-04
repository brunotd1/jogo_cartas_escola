using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Permite arrastar e soltar (Drag and Drop) a carta na tela.
/// Se for solta em uma casa válida do tabuleiro (BoardSlot) e o jogador tiver mantimentos suficientes,
/// a carta é colocada no campo. Caso contrário, ela volta suavemente para a mão.
/// </summary>
[RequireComponent(typeof(CardDisplay))]
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CardDisplay cardDisplay;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector3 originalPosition;
    private int originalSiblingIndex;

    public bool isPlacedOnBoard = false;

    void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Se a carta já estiver posicionada no tabuleiro, não pode mais ser arrastada como mão
        if (isPlacedOnBoard) return;

        originalParent = transform.parent;
        originalPosition = transform.position;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Faz a carta renderizar na frente de tudo durante o arrasto
        transform.SetParent(canvas.transform);

        // Desativa bloqueio de raios para que o mouse consiga "enxergar" o BoardSlot embaixo da carta
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlacedOnBoard) return;

        // Move a carta acompanhando o cursor do mouse
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlacedOnBoard) return;

        // Restaura opacidade e detecção de clique
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        // Se não foi posicionada em nenhum slot válido, retorna para a mão
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
        transform.position = originalPosition;
    }

    /// <summary>
    /// Chamado pelo BoardSlot quando a carta é solta sobre ele.
    /// </summary>
    public void TryPlaceOnSlot(BoardSlot slot)
    {
        if (isPlacedOnBoard) return;

        // 1. Verifica se a regra do tabuleiro permite colocar esta carta nesta casa
        if (!slot.CanPlaceCard(cardDisplay))
        {
            Debug.LogWarning($"Não é permitido colocar {cardDisplay.cardData.cardName} nesta linha!");
            return;
        }

        // 2. Verifica se o jogador tem mantimentos suficientes (via GameManager se existir)
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.CanAfford(cardDisplay.cardData.supplyCost, cardDisplay.isPlayerCard))
            {
                Debug.LogWarning("Mantimentos insuficientes para invocar esta carta!");
                return;
            }

            // Gasta os mantimentos
            GameManager.Instance.SpendSupplies(cardDisplay.cardData.supplyCost, cardDisplay.isPlayerCard);
        }

        // 3. Sucesso! Fixa a carta no slot
        isPlacedOnBoard = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        slot.PlaceCard(cardDisplay);
    }
}
