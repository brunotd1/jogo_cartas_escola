using UnityEngine;
using TMPro;

/// <summary>
/// O Cérebro do Jogo (GameManager).
/// Controla o fluxo de turnos, o aumento de mantimentos (1 a 10),
/// a vida dos jogadores (20 HP) e a interface geral da partida.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Vida dos Jogadores (Início: 20 HP)")]
    public int playerHealth = 20;
    public int enemyHealth = 20;

    [Header("Sistema de Mantimentos (Recurso: 1 a 10)")]
    [Tooltip("Ative para testar todas as cartas livremente sem travar por falta de mantimentos")]
    public bool infiniteSuppliesForTesting = true;

    [Tooltip("Capacidade máxima atual de mantimentos do jogador")]
    public int playerMaxSupplies = 1;
    [Tooltip("Mantimentos disponíveis para gastar no turno atual")]
    public int playerCurrentSupplies = 1;

    public int enemyMaxSupplies = 1;
    public int enemyCurrentSupplies = 1;

    [Header("Controle de Turnos")]
    public int turnCount = 1;
    public bool isPlayerTurn = true;

    [Header("Interface Geral (UI)")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI playerSuppliesText;
    public TextMeshProUGUI turnText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Inicia a partida com 20 de vida e 1 mantimento.
    /// </summary>
    public void StartGame()
    {
        playerHealth = 20;
        enemyHealth = 20;

        turnCount = 1;
        playerMaxSupplies = 1;
        playerCurrentSupplies = playerMaxSupplies;

        enemyMaxSupplies = 1;
        enemyCurrentSupplies = enemyMaxSupplies;

        isPlayerTurn = true;

        UpdateUI();
        Debug.Log("Partida iniciada! Ambos jogadores com 20 de vida e 1 de mantimento.");
    }

    /// <summary>
    /// Verifica se o jogador/inimigo tem mantimentos suficientes para invocar a carta.
    /// </summary>
    public bool CanAfford(int cost, bool isPlayer)
    {
        if (infiniteSuppliesForTesting) return true;

        if (isPlayer)
            return playerCurrentSupplies >= cost;
        else
            return enemyCurrentSupplies >= cost;
    }

    /// <summary>
    /// Deduz os mantimentos quando uma carta é jogada.
    /// </summary>
    public void SpendSupplies(int cost, bool isPlayer)
    {
        if (isPlayer)
        {
            playerCurrentSupplies -= cost;
            if (playerCurrentSupplies < 0) playerCurrentSupplies = 0;
        }
        else
        {
            enemyCurrentSupplies -= cost;
            if (enemyCurrentSupplies < 0) enemyCurrentSupplies = 0;
        }

        UpdateUI();
    }

    /// <summary>
    /// Finaliza o turno atual e avança para o próximo.
    /// A cada novo turno do jogador, o total de mantimentos aumenta em 1 até o máximo de 10.
    /// </summary>
    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            turnCount++;

            // O número de mantimentos aumenta em 1 por turno até o teto de 10
            if (playerMaxSupplies < 10)
                playerMaxSupplies++;

            // Recarrega todos os mantimentos para o novo turno
            playerCurrentSupplies = playerMaxSupplies;

            Debug.Log($"Turno {turnCount} do Jogador! Mantimentos recarregados: {playerCurrentSupplies}/{playerMaxSupplies}");
        }
        else
        {
            // Turno do oponente
            if (enemyMaxSupplies < 10)
                enemyMaxSupplies++;

            enemyCurrentSupplies = enemyMaxSupplies;
            Debug.Log($"Turno do Oponente! Mantimentos: {enemyCurrentSupplies}/{enemyMaxSupplies}");
        }

        UpdateUI();
    }

    /// <summary>
    /// Aplica dano direto ao Herói / Jogador.
    /// </summary>
    public void DamagePlayer(int amount)
    {
        playerHealth -= amount;
        if (playerHealth < 0) playerHealth = 0;

        UpdateUI();

        if (playerHealth <= 0)
        {
            Debug.Log("Derrota! O jogador perdeu todos os 20 pontos de vida.");
        }
    }

    /// <summary>
    /// Aplica dano direto ao Oponente.
    /// </summary>
    public void DamageEnemy(int amount)
    {
        enemyHealth -= amount;
        if (enemyHealth < 0) enemyHealth = 0;

        UpdateUI();

        if (enemyHealth <= 0)
        {
            Debug.Log("Vitória! O oponente foi derrotado!");
        }
    }

    /// <summary>
    /// Atualiza os textos de vida, mantimentos e turno na tela.
    /// </summary>
    public void UpdateUI()
    {
        if (playerHealthText != null)
            playerHealthText.text = $"Vida: {playerHealth}";

        if (enemyHealthText != null)
            enemyHealthText.text = $"Inimigo: {enemyHealth}";

        if (playerSuppliesText != null)
            playerSuppliesText.text = $"Mantimentos: {playerCurrentSupplies}/{playerMaxSupplies}";

        if (turnText != null)
            turnText.text = isPlayerTurn ? $"Turno {turnCount} (Sua Vez)" : $"Turno {turnCount} (Vez do Oponente)";
    }
}
