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
        /// <summary>
        /// Invoked when the player has ended the turn.
        /// </summary>
        public event Action TurnEnded;

        /// <summary>
        /// The index of the player in GameplayManager.
        /// </summary>
        public int Id { get; set; }

        protected abstract PlayerType Type { get; }
        protected IEnumerable<Unit> UnitsWithEnergy
        {
            get => m_myUnits.Where(unit => unit.Energy.sqrMagnitude > 0);
        }

        protected List<Unit>        m_myUnits;
        protected GameplayUIManager m_gameplayUIManager;
        protected GridRenderer      m_gridRenderer;

        /// <summary>
        /// Initializes the player and adds the listener so the player knows when a unit is dead from its side.
        /// </summary>
        public Playerbase(List<Unit> units, GameplayUIManager manager, GridRenderer gridRenderer)
        {
            this.m_myUnits = units;
            this.m_gameplayUIManager = manager;
            this.m_gridRenderer = gridRenderer;
            foreach(var unit in units) unit.UnitDead += () => OnUnitDead(unit);
        }

        /// <summary>
        /// Starts the player turn, resets its unit energy and gets the UI ready based on its type.
        /// </summary>
        public virtual void StartTurn()
        {
            foreach(var unit in m_myUnits) unit.ResetEnergy();
            this.m_gameplayUIManager.TurnStarted(Type);
            this.m_gameplayUIManager.SkipButtonPressed += EndTurn;
        }

        /// <summary>
        /// Ends Player turn and gives control to the GameplayManager to give control to the next player.
        /// </summary>
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
        Human
    }
}
