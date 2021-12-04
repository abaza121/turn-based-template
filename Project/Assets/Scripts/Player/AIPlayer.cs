using System.Collections;
using System.Collections.Generic;
using TurnBased.Gameplay;
using UnityEngine;

namespace TurnBased.Player
{
    public class AIPlayer : Playerbase
    {
        public AIPlayer(List<Unit> units) : base(units)
        {
        }

        public override void StartTurn()
        {
            throw new System.NotImplementedException();
        }
    }
}
