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
        [SerializeField] InitialSituation  initialSituation;
        [SerializeField] UnitsFactory      unitsFactory;
        [SerializeField] GridRenderer      gridRenderer;
        [SerializeField] PlayerType[]      playerTypes;

        GridData     m_currentGridData;
        Playerbase[] m_gamePlayers;
        bool[]       m_livingPlayers;
        int          m_currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
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
            this.m_gamePlayers = new Playerbase[playerTypes.Length];
            this.m_livingPlayers = new bool[playerTypes.Length];
            for(int i = 0;i<playerTypes.Length;i++)
            {
                var extractedUnits = units.Where(x => x.OwningPlayer == i).ToList();
                if(extractedUnits.Count > 0) this.m_livingPlayers[i] = true;
                switch(playerTypes[i])
                {
                    case PlayerType.Hooman:
                        this.m_gamePlayers[i] = new HumanPlayer(this.gridRenderer, gameplayUIManager, extractedUnits);
                        break;
                    case PlayerType.AI:
                        this.m_gamePlayers[i] = new AIPlayer(extractedUnits, gameplayUIManager, m_currentGridData, this.gridRenderer);
                        break;
                    case PlayerType.SmartAI:
                        this.m_gamePlayers[i] = new SmartAIPlayer(extractedUnits, gameplayUIManager, m_currentGridData, this.gridRenderer);
                        break;
                }

                this.m_gamePlayers[i].Id = i;
            }

            this.m_gamePlayers[m_currentPlayer].TurnEnded += OnPlayerTurnEnded;
            this.m_gamePlayers[m_currentPlayer].StartTurn(); // Start turn for the first player.
        }

        void OnPlayerTurnEnded()
        {
            this.m_gamePlayers[m_currentPlayer].TurnEnded -= OnPlayerTurnEnded;
            do
            {
                ++m_currentPlayer;
                if (m_currentPlayer >= m_gamePlayers.Length) m_currentPlayer = 0;
            }
            while (!m_livingPlayers[m_currentPlayer]); // Check for next living player.
            this.m_gamePlayers[m_currentPlayer].TurnEnded += OnPlayerTurnEnded;
            this.m_gamePlayers[m_currentPlayer].StartTurn();
        }

        void OnUnitDead(Unit unit)
        {
            m_currentGridData.CurrentUnits.Remove(unit);
            GameObject.Destroy(unit.gameObject);
            this.EvaluateGameState();
        }

        void EvaluateGameState()
        {
            for(int i = 0;i< m_gamePlayers.Length;i++)
            {
                if (m_currentGridData.CurrentUnits.Any(x => x.OwningPlayer == i))
                    m_livingPlayers[i] = true;
                else
                    m_livingPlayers[i] = false;
            }

            if (m_livingPlayers.Count(x => x == true) == 1)
            {
                for(int i = 0;i< m_livingPlayers.Length;i++)
                    if(m_livingPlayers[i]) this.gameplayUIManager.ShowEndGamePanel(m_gamePlayers[i].Id);
            }
   
        }
    }
}
