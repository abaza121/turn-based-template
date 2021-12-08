using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnBased.Gameplay;
using TurnBased.Grids;
using TurnBased.UI;
using TurnBased.Utils;

namespace TurnBased.Player
{
    /// <summary>
    /// A Smarter AI player with deterministic moves instead of random ones.
    /// </summary>
    public class SmartAIPlayer : AIPlayer
    {
        public SmartAIPlayer(List<Unit> units, GameplayUIManager manager, GridData data, GridRenderer gridRenderer) : base(units, manager, data, gridRenderer) { }

        protected override IEnumerator TurnCoroutine()
        {
            foreach (var unit in UnitsWithEnergy)
            {
                yield return this.AttackIfPossible(unit);
                yield return this.DeterministicMove(unit);
                if (!unit.UnitAttacked) yield return this.AttackIfPossible(unit);
            }

            EndTurn();
        }

        /// <summary> Moves the given unit towards closest enemy. </summary>
        private IEnumerator DeterministicMove(Unit unit)
        {
            var possiblePositions = unit.CurrentCellPosition.GetPositionsAtRange(unit.Energy).Where(x => this.m_gridRenderer.IsEmptyAtPos(x));
            var enemyPositions    = this.m_gridData.CurrentUnits.Where(x => x.OwningPlayer != this.Id).Select(x => x.CurrentCellPosition);
            var lowestDistance    = int.MaxValue;
            var chosenPosition    = unit.CurrentCellPosition;

            // Get Closest Position to Possible Positions.
            foreach (var position in enemyPositions)
                foreach (var possiblePosition in possiblePositions)
                {
                    var dstCompare = (position - possiblePosition).sqrMagnitude;
                    if (dstCompare < lowestDistance)
                    {
                        chosenPosition = possiblePosition;
                        lowestDistance = dstCompare;
                    }
                }

            // Move unit to the new position.
            if (chosenPosition != unit.CurrentCellPosition)
            {
                var targetCell = this.m_gridRenderer.GetCellAtPos(chosenPosition);
                var currentCell = this.m_gridRenderer.GetCellAtPos(unit.CurrentCellPosition);
                var wait = true;
                unit.MoveUnitFromTo(currentCell, targetCell, () => wait = false);
                while (wait) yield return null;
            }
        }
    }
}
