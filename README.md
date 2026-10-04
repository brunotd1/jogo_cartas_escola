# ⚔️ Jogo de Cartas e Tabuleiro Medieval (Gwent + Hearthstone)

Projeto desenvolvido na Unity com temática medieval tática, combinando posicionamento em tabuleiro em grade 4x5 e gestão de recursos de mantimentos.

## 📌 Visão Geral do Jogo
* **Modo:** 1x1 (Jogador vs Oponente).
* **Tabuleiro:** Grade 4x5 (20 casas), dividida entre Linha de Frente (Corpo a Corpo) e Retaguarda (Distância / Suporte).
* **Recursos:** Mantimentos iniciam em 1 e aumentam em +1 a cada turno até o limite de 10.
* **Pontos de Vida:** Cada jogador inicia com 20 de vida.
* **Combate:** Tropas corpo a corpo trocam dano simultaneamente.

## 🏗️ Estrutura Implementada
* `CardData.cs`: Molde ScriptableObject para criação de cartas modulares.
* `CardDisplay.cs`: Gerenciador visual da carta e controle de vida/ataque em tempo real.
* `CardDragHandler.cs`: Sistema de arraste e solte da mão para as casas válidas.
* `Board.cs`: Gerador automático e gerenciador do tabuleiro 4x5.
* `BoardSlot.cs`: Controle de cada casa individual do tabuleiro com regras de linha.
* `GameManager.cs`: Gerenciador de vida, ciclo de turnos e mantimentos.

## 🎯 Próximos Passos
1. Sistema de seleção de ataque e troca de dano entre cartas.
2. Contadores visuais na tela (Vida 20 e Mantimentos 1/10) e botão de Passar Turno.
3. Baralho (Deck) e saque automático de cartas.
4. Lógica de IA para ações do oponente.
