using System.Collections.Generic;
using System.Linq;
using TurnBased.Data;
using TurnBased.Grids;
using TurnBased.Player;
using TurnBased.UI;
using UnityEngine;

namespace TurnBased.Gameplay
{
    /// <summary>
    /// Controls Gameplay loop, giving turns and determining who wins.
    /// </summary>
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] GameplayUIManager gameplayUIManager;
        [SerializeField] UnitsFactory      unitsFactory;
        [SerializeField] GridRenderer      gridRenderer;

        GridData         m_currentGridData;
        List<Playerbase> m_gamePlayers;
        int              m_currentPlayer;

        /// <summary>
        /// Initialize Game play data and start turn for the first player.
        /// </summary>
        void Start()
        {
            var initialSituation = GameManager.Instance.ChosenInitalSituation;
            // Initialize And Verify Data.
            if (!GridInitializer.TryGenerateGridData(initialSituation, out m_currentGridData)) return;
            if (!UnitInitializer.GenerateUnits(initialSituation, unitsFactory, out var units)) return;

            // Initialize Units.
            foreach (var unit in units) unit.UnitDead += () => OnUnitDead(unit);
            m_currentGridData.CurrentUnits = units;
            this.gameplayUIManager.InitializeHealthbars(units, m_currentGridData.CellSize);

            // Generate Cells.
            GridInitializer.GenerateGridCells(m_currentGridData, gridRenderer);

            // Initialize Players.
            var playerTypes = initialSituation.Players;
            this.m_gamePlayers = new List<Playerbase>();
            for(int i = 0;i<playerTypes.Length;i++)
            {
                var extractedUnits = units.Where(x => x.OwningPlayer == i).ToList();
                if(extractedUnits.Count == 0) continue;
                switch(playerTypes[i])
                {
                    case PlayerType.Hooman:
                        this.m_gamePlayers.Add(new HumanPlayer(this.gridRenderer, gameplayUIManager, extractedUnits));
                        break;
                    case PlayerType.AI:
                        this.m_gamePlayers.Add(new AIPlayer(extractedUnits, gameplayUIManager, m_currentGridData, this.gridRenderer));
                        break;
                    case PlayerType.SmartAI:
                        this.m_gamePlayers.Add(new SmartAIPlayer(extractedUnits, gameplayUIManager, m_currentGridData, this.gridRenderer));
                        break;
                }

                this.m_gamePlayers[i].Id = i;
            }

            this.m_gamePlayers[m_currentPlayer].TurnEnded += OnPlayerTurnEnded;
            this.m_gamePlayers[m_currentPlayer].StartTurn(); // Start turn for the first player.
        }

        /// <summary>
        /// Handles Player turn end, Starts the next living player turn.
        /// </summary>
        void OnPlayerTurnEnded()
        {
            this.m_gamePlayers[m_currentPlayer].TurnEnded -= OnPlayerTurnEnded;
            ++m_currentPlayer;
            if (m_currentPlayer >= m_gamePlayers.Count) m_currentPlayer = 0;
            this.m_gamePlayers[m_currentPlayer].TurnEnded += OnPlayerTurnEnded;
            this.m_gamePlayers[m_currentPlayer].StartTurn();
        }

        /// <summary>
        /// When a Unit is dead remove it from current units list.
        /// </summary>
        void OnUnitDead(Unit unit)
        {
            m_currentGridData.CurrentUnits.Remove(unit);
            GameObject.Destroy(unit.gameObject);
            this.EvaluateGameState();
        }

        /// <summary>
        /// Evaluate the state of current game and remove dead players, show game end panel when only one player is alive.
        /// </summary>
        void EvaluateGameState()
        {
            for(int i = 0;i< m_gamePlayers.Count;i++)
            {
                if (!m_currentGridData.CurrentUnits.Any(x => x.OwningPlayer == i)) this.m_gamePlayers.RemoveAt(i);
            }

            if (this.m_gamePlayers.Count == 1) this.gameplayUIManager.ShowEndGamePanel(this.m_gamePlayers[0].Id);
        }
    }
}
