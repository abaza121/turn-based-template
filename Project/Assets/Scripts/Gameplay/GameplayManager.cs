using System.Linq;
using TurnBased.Data;
using TurnBased.Grids;
using TurnBased.Player;
using UnityEngine;

namespace TurnBased.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private InitialSituation initialSituation;
        [SerializeField] private UnitsFactory     unitsFactory;
        [SerializeField] private GridRenderer     gridRenderer;
        [SerializeField] private PlayerType[]     playerTypes;

        GridData     m_currentGridData;
        Playerbase[] m_gamePlayers;
        int          m_currentPlayer;

        // Start is called before the first frame update
        void Start()
        {
            if (!GridInitializer.TryGenerateGridData(initialSituation, out m_currentGridData)) return;
            if (!UnitInitializer.GenerateUnits(initialSituation, unitsFactory, out var units)) return;
            m_currentGridData.CurrentUnits = units;
            GridInitializer.GenerateGridCells(m_currentGridData, gridRenderer);
            this.m_gamePlayers = new Playerbase[playerTypes.Length];

            for(int i = 0;i<playerTypes.Length;i++)
            {
                var extractedUnits = units.Where(x => x.OwningPlayer == i).ToList();
                switch(playerTypes[i])
                {
                    case PlayerType.Hooman:
                        this.m_gamePlayers[i] = new HumanPlayer(this.gridRenderer, extractedUnits);
                        break;
                    case PlayerType.AI:
                        this.m_gamePlayers[i] = new AIPlayer(extractedUnits);
                        break;
                }

                this.m_gamePlayers[i].Id = i;
            }

            this.m_gamePlayers[m_currentPlayer].TurnEnded += OnPlayerTurnEnded;
            this.m_gamePlayers[m_currentPlayer].StartTurn();
        }

        private void OnPlayerTurnEnded()
        {
            throw new System.NotImplementedException();
        }
    }
}
