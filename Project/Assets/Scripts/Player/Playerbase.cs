using System;
using System.Collections.Generic;
using TurnBased.Gameplay;
using TurnBased.Grids;

namespace TurnBased.Player
{
    public abstract class Playerbase
    {
        public event Action TurnEnded;
        public int Id { get; set; }
        protected List<Unit> m_myUnits;

        public Playerbase(List<Unit> units)
        {
            this.m_myUnits = units;
        }

        public abstract void StartTurn();
        public virtual void EndTurn()
        {
            TurnEnded.Invoke();
        }
    }

    public enum PlayerType
    {
        AI,
        Hooman
    }
}
