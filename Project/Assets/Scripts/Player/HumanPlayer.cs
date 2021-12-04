using System.Collections.Generic;
using TurnBased.Gameplay;
using TurnBased.Grids;
using UnityEngine;

namespace TurnBased.Player
{
    public class HumanPlayer : Playerbase
    {
        private GridRenderer _gridRenderer;
        private Cell         _currentSelectedCell;
        private bool         _disableInput;

        public HumanPlayer(GridRenderer gridRenderer, List<Unit> units) : base(units)
        {
            this._gridRenderer = gridRenderer;
        }

        public override void StartTurn()
        {
            this._gridRenderer.ShowCellsToChooseForUnits(this.m_myUnits);
            this._gridRenderer.CellSelected += this.OnCellSelected;
        }

        void OnCellSelected(Cell cell)
        {
            if (_disableInput) return;
            if (_currentSelectedCell != null)
            {
                this.OnChoosingAciton(cell);
                return;
            }

            if (cell.OccupyingUnit == null) this._gridRenderer.ShowCellsToChooseForUnits(this.m_myUnits);
            else if (cell.OccupyingUnit.OwningPlayer == this.Id)
            {
                this._gridRenderer.ShowActionsForUnit(cell.OccupyingUnit);
                _currentSelectedCell = cell;
            }
        }

        void OnChoosingAciton(Cell cell)
        {
            switch(cell.CurrentHighlightMode)
            {
                case CellHighlightMode.NotHighlighted:
                    this._gridRenderer.ShowCellsToChooseForUnits(this.m_myUnits);
                    break;
                case CellHighlightMode.CanChooseIt:
                    this._currentSelectedCell = cell;
                    this._gridRenderer.ShowActionsForUnit(cell.OccupyingUnit);
                    return;
                case CellHighlightMode.CanMoveTo:
                    this._disableInput = true;
                    this._gridRenderer.ResetAll();
                    this._currentSelectedCell.OccupyingUnit.MoveUnitFromTo(this._currentSelectedCell, cell, () =>
                    {
                        this._disableInput = false;
                        this._gridRenderer.ShowCellsToChooseForUnits(this.m_myUnits, false);
                    });
                    break;
                case CellHighlightMode.CanAttack:
                    this._currentSelectedCell.OccupyingUnit.AttackCell(cell);
                    break;
            }

            this._currentSelectedCell = null;
        }
    }
}
