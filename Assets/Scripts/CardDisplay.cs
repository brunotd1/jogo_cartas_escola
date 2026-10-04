using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Responsável por dar vida visual à carta no jogo.
/// Ele lê os dados do CardData (ScriptableObject) e atualiza os textos, imagens
/// e controla a vida e ataque atuais da carta durante a partida.
/// </summary>
public class CardDisplay : MonoBehaviour
{
    [Header("Referência dos Dados da Carta")]
    public CardData cardData;

    [Header("Elementos de Interface (UI)")]
    [Tooltip("Texto do nome da carta")]
    public TextMeshProUGUI nameText;

    [Tooltip("Texto da descrição / habilidade")]
    public TextMeshProUGUI descriptionText;

    [Tooltip("Imagem da arte da carta")]
    public Image artImage;

    [Tooltip("Texto com o custo de mantimentos (1 a 10)")]
    public TextMeshProUGUI costText;

    [Tooltip("Texto com o poder de ataque")]
    public TextMeshProUGUI attackText;

    [Tooltip("Texto com os pontos de vida")]
    public TextMeshProUGUI healthText;

    [Tooltip("Texto ou ícone indicando o tipo (Melee, Ranged, Support)")]
    public TextMeshProUGUI typeText;

    [Header("Estado Atual em Jogo (Valores em Tempo Real)")]
    public int currentHealth;
    public int currentAttack;
    public bool isPlayerCard = true; // Define se pertence ao jogador ou ao oponente

    void Start()
    {
        if (cardData != null)
        {
            SetupCard(cardData, isPlayerCard);
        }
    }

    /// <summary>
    /// Inicializa a carta com os dados informados e atualiza a parte visual.
    /// </summary>
    public void SetupCard(CardData data, bool playerOwned = true)
    {
        cardData = data;
        isPlayerCard = playerOwned;

        currentHealth = data.maxHealth;
        currentAttack = data.attack;

        UpdateVisuals();
    }

    /// <summary>
    /// Atualiza os textos e imagens na tela com base nos valores atuais.
    /// </summary>
    public void UpdateVisuals()
    {
        if (cardData == null) return;

        if (nameText != null) 
            nameText.text = cardData.cardName;

        if (descriptionText != null) 
            descriptionText.text = cardData.description;

        if (costText != null) 
            costText.text = cardData.supplyCost.ToString();

        if (attackText != null) 
            attackText.text = currentAttack.ToString();

        if (healthText != null) 
            healthText.text = currentHealth.ToString();

        if (typeText != null)
        {
            switch (cardData.cardType)
            {
                case CardType.Melee:
                    typeText.text = "Corpo a Corpo";
                    break;
                case CardType.Ranged:
                    typeText.text = "Distância";
                    break;
                case CardType.Support:
                    typeText.text = "Suporte";
                    break;
            }
        }

        if (artImage != null && cardData.cardArt != null)
        {
            artImage.sprite = cardData.cardArt;
            artImage.enabled = true;
        }
    }

    /// <summary>
    /// Aplica dano à carta e atualiza o texto de vida.
    /// Se a vida chegar a zero ou menos, a carta é destruída.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateVisuals();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destrói a carta na tela quando sua vida chega a zero.
    /// </summary>
    void Die()
    {
        Debug.Log($"A carta {cardData.cardName} foi derrotada em combate!");
        Destroy(gameObject);
    }
}
