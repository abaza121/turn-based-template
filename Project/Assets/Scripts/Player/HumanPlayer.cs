using System.Collections.Generic;
using System.Linq;
using TurnBased.Gameplay;
using TurnBased.Grids;
using TurnBased.UI;

namespace TurnBased.Player
{
    /// <summary>
    /// Gives input control to the human player once he gets the turn.
    /// </summary>
    public class HumanPlayer : Playerbase
    {
        private Cell m_currentSelectedCell;
        private bool m_disableInput;

        public HumanPlayer(GridRenderer gridRenderer, GameplayUIManager manager, List<Unit> units) : base(units, manager, gridRenderer)
        {
            this.m_gridRenderer      = gridRenderer;
        }

        protected override PlayerType Type => PlayerType.Human;

        /// <summary>
        /// Show units that can be chosen and wait for cell selected event.
        /// </summary>
        public override void StartTurn()
        {
            base.StartTurn();
            this.m_gridRenderer.ShowCellsToChooseForUnits(this.UnitsWithEnergy);
            this.m_gridRenderer.CellSelected += this.OnCellSelected;
            if(this.UnitsWithEnergy.Count() == 0) this.m_gameplayUIManager.ReadyToEndTurn();
        }

        /// <summary>
        /// Clear Cell Selected Event and propagate end turn to the gameplay manager.
        /// </summary>
        public override void EndTurn()
        {
            this.m_gridRenderer.CellSelected -= this.OnCellSelected;
            this.m_gridRenderer.ResetAll();
            base.EndTurn();
        }

        /// <summary>
        /// When a cell is selected check what type of unit in cell and show available action if allied unit.
        /// </summary>
        void OnCellSelected(Cell cell)
        {
            if (m_disableInput) return;
            if (m_currentSelectedCell != null)
            {
                this.OnChoosingAciton(cell);
                return;
            }

            if (cell.OccupyingUnit == null) this.m_gridRenderer.ShowCellsToChooseForUnits(this.UnitsWithEnergy);
            else if (cell.OccupyingUnit.OwningPlayer == this.Id && cell.OccupyingUnit.Energy.sqrMagnitude > 0)
            {
                this.m_gridRenderer.ShowActionsForUnit(cell.OccupyingUnit);
                m_currentSelectedCell = cell;
            }
        }

        void OnChoosingAciton(Cell cell)
        {
            switch(cell.CurrentHighlightMode)
            {
                case CellHighlightMode.NotHighlighted:
                    this.m_gridRenderer.ShowCellsToChooseForUnits(this.UnitsWithEnergy);
                    break;
                case CellHighlightMode.CanChooseIt:
                    this.m_currentSelectedCell = cell;
                    this.m_gridRenderer.ShowActionsForUnit(cell.OccupyingUnit);
                    return;
                case CellHighlightMode.CanMoveTo:
                    this.m_disableInput = true;
                    this.m_gridRenderer.ResetAll();
                    this.m_gameplayUIManager.ChangeSkipButtonState(false);
                    this.m_currentSelectedCell.OccupyingUnit.MoveUnitFromTo(this.m_currentSelectedCell, cell, () =>
                    {
                        this.m_disableInput = false;
                        this.m_gridRenderer.ShowCellsToChooseForUnits(this.UnitsWithEnergy, false);
                        this.m_gameplayUIManager.ChangeSkipButtonState(true);
                    });
                    break;
                case CellHighlightMode.CanAttack:
                    this.m_disableInput = true;
                    this.m_gridRenderer.ResetAll();
                    this.m_currentSelectedCell.OccupyingUnit.AttackUnit(cell.OccupyingUnit);
                    this.m_gameplayUIManager.ChangeSkipButtonState(false);
                    this.m_gridRenderer.ShowExplosion(cell, () =>
                    {
                        this.m_disableInput = false;
                        this.m_gridRenderer.ShowCellsToChooseForUnits(this.UnitsWithEnergy, false);
                        this.m_gameplayUIManager.ChangeSkipButtonState(true);
                    });
                    break;
            }

            if (this.UnitsWithEnergy.Count() == 0) this.m_gameplayUIManager.ReadyToEndTurn();
            this.m_currentSelectedCell = null;
        }
    }
}
