using System;
using System.Collections.Generic;
using TurnBased.Gameplay;
using System.Linq;
using TurnBased.UI;
using TurnBased.Grids;

namespace TurnBased.Player
{
    /// <summary>
    /// Base class for player, controls turn loop once it got the turn.
    /// </summary>
    public abstract class Playerbase
    {
        public event Action TurnEnded;
        public int Id { get; set; }

        protected abstract PlayerType Type { get; }
        protected IEnumerable<Unit> UnitsWithEnergy
        {
            get => m_myUnits.Where(unit => unit.Energy.sqrMagnitude > 0);
        }

        protected List<Unit>        m_myUnits;
        protected GameplayUIManager m_gameplayUIManager;
        protected GridRenderer      m_gridRenderer;

        public Playerbase(List<Unit> units, GameplayUIManager manager, GridRenderer gridRenderer)
        {
            this.m_myUnits = units;
            this.m_gameplayUIManager = manager;
            this.m_gridRenderer = gridRenderer;
            foreach(var unit in units) unit.UnitDead += () => OnUnitDead(unit);
        }

        public virtual void StartTurn()
        {
            foreach(var unit in m_myUnits) unit.ResetEnergy();
            this.m_gameplayUIManager.TurnStarted(Type);
            this.m_gameplayUIManager.SkipButtonPressed += EndTurn;
        }

        public virtual void EndTurn()
        {
            TurnEnded.Invoke();
            this.m_gameplayUIManager.SkipButtonPressed -= EndTurn;
        }

        void OnUnitDead(Unit unit) => m_myUnits.Remove(unit);
    }

    public enum PlayerType
    {
        AI,
        SmartAI,
        Hooman
    }
}
