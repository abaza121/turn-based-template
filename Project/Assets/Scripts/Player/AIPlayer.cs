using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnBased.Gameplay;
using TurnBased.Grids;
using TurnBased.UI;
using TurnBased.Utils;
using UnityEngine;

namespace TurnBased.Player
{
    /// <summary>
    /// Controls AI Player behavior, making decisions and ending turns.
    /// </summary>
    public class AIPlayer : Playerbase
    {
        protected GridData m_gridData;
        protected override PlayerType Type => PlayerType.AI;

        public AIPlayer(List<Unit> units, GameplayUIManager manager, GridData data, GridRenderer gridRenderer) : base(units, manager, gridRenderer)
        {
            this.m_gridData = data;
        }

        public override void StartTurn()
        {
            base.StartTurn();
            this.m_gridRenderer.StartCoroutine(this.TurnCoroutine());
        }

        protected virtual IEnumerator TurnCoroutine()
        {
            foreach (var unit in UnitsWithEnergy)
            {
                yield return this.AttackIfPossible(unit);
                yield return this.MoveRandomly(unit);
                if(!unit.UnitAttacked) yield return this.AttackIfPossible(unit);
            }

            EndTurn();
        }

        protected IEnumerator AttackIfPossible(Unit unit)
        {
            var possiblePositions = unit.CurrentCellPosition.GetPositionsAtRange(unit.AttackRange);
            foreach (var enemyUnit in this.m_gridData.CurrentUnits.Where(x => x.OwningPlayer != this.Id))
                if(possiblePositions.Contains(enemyUnit.CurrentCellPosition))
                {
                    unit.AttackUnit(enemyUnit);
                    var wait = true;
                    this.m_gridRenderer.ShowExplosion(enemyUnit.CurrentCellPosition, () => wait = false);
                    while (wait) yield return null;
                    break;
                }
        }

        IEnumerator MoveRandomly(Unit unit)
        {
            var possiblePositions = unit.CurrentCellPosition.GetPositionsAtRange(unit.Energy).Where(pos => !this.m_gridRenderer.IsOutOfBounds(pos) && this.m_gridRenderer.GetCellAtPos(pos).OccupyingUnit == null).ToList();
            if (possiblePositions.Count == 0) yield break;

            var randomPosition    = possiblePositions[Random.Range(0, possiblePositions.Count)];
            var targetCell        = this.m_gridRenderer.GetCellAtPos(randomPosition);
            var currentCell       = this.m_gridRenderer.GetCellAtPos(unit.CurrentCellPosition);
            var wait = true;
            unit.MoveUnitFromTo(currentCell, targetCell, () => wait = false);
            while (wait) yield return null;
        }
    }
}
