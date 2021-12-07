using System;
using System.Collections;
using System.Collections.Generic;
using TurnBased.Gameplay;
using TurnBased.Utils;
using UnityEngine;

namespace TurnBased.Grids
{
    /// <summary>
    /// Responsible for controlling cells and rendering different events.
    /// </summary>
    public class GridRenderer : MonoBehaviour
    {
        /// <summary>
        /// Invoked when a cell is selected.
        /// </summary>
        public event     Action<Cell> CellSelected;

        [SerializeField] Cell         cellPrefab;
        [SerializeField] GameObject   explosion;

        Array    m_cells;
        Vector3  m_startPosition;

        /// <summary>
        /// Initializes the cells based on the given grid data.
        /// </summary>
        public void InitializeGridCells(GridData data)
        {
            this.m_cells         = Array.CreateInstance(typeof(Cell), new int[2] {data.ColumnsCount, data.RowsCount}, new int[2] { 1, 1 });                                                     // A special array with indexing that starts at 1.
            this.m_startPosition = this.transform.position - (new Vector3((data.ColumnsCount * data.CellSize - data.CellSize), 0, (data.RowsCount * data.CellSize - data.CellSize)) * 0.5f);    // The start position for the grid to start populating cells at.
            this.explosion.transform.GetChild(0).localScale = Vector3.one * 0.5f * data.CellSize;                                                                                               // Set the scale for the explosion based on Cell Size.

            // Generate Cells.
            for(int i = 0;i< data.ColumnsCount; i++)
                for(int j = 0;j< data.RowsCount; j++)
                    InitializeCellAtPos(i, j, data.CellSize);

            // Populate Units.
            foreach(var unit in data.CurrentUnits)
            {
                var targetCell = this.GetCellAtPos(unit.CurrentCellPosition);
                var targetRotation = this.transform.position - targetCell.transform.position;
                unit.InitializeUnitAtCell(targetCell, data.CellSize, targetRotation);
            }
        }

        /// <summary>
        /// Highlight the cells that have the given units.
        /// </summary>
        public void ShowCellsToChooseForUnits(IEnumerable<Unit> units, bool resetAll = true)
        {
            if(resetAll) ResetAll();
            foreach (var unit in units) GetCellAtPos(unit.CurrentCellPosition).ChangeHighlight(CellHighlightMode.CanChooseIt);
        }

        /// <summary>
        /// Resets all cells.
        /// </summary>
        public void ResetAll()
        {
            // Not the best I would say wouldn't scale well for extra big maps, a solution would be to cached changed cell, might come back to it later.
            for (int i = 1; i <= m_cells.GetUpperBound(0); i++)
                for (int j = 1; j <= m_cells.GetUpperBound(1); j++)
                    this.GetCellAtPos(i,j).ChangeHighlight(CellHighlightMode.NotHighlighted);
        }

        /// <summary>
        /// Shows available action for the given unit.
        /// </summary>
        public void ShowActionsForUnit(Unit unit)
        {
            this.ResetAll();
            var attackRangeList = unit.CurrentCellPosition.GetPositionsAtRange(unit.AttackRange);
            foreach(var position in attackRangeList)
            {
                if (this.IsOutOfBounds(position)) continue;
                var targetCell = this.GetCellAtPos(position);
                if (targetCell.OccupyingUnit == null) continue;
                if (targetCell.OccupyingUnit.OwningPlayer != unit.OwningPlayer) targetCell.ChangeHighlight(CellHighlightMode.CanAttack);
            }

            var moveRangeList = unit.CurrentCellPosition.GetPositionsAtRange(unit.Energy);
            foreach (var position in moveRangeList)
            {
                if (this.IsOutOfBounds(position)) continue;
                var targetCell = this.GetCellAtPos(position);
                if (targetCell.OccupyingUnit == null) targetCell.ChangeHighlight(CellHighlightMode.CanMoveTo);
            }
        }

        /// <summary> Gets cell at the given position. </summary>
        public Cell GetCellAtPos(Vector2Int pos)                          => this.GetCellAtPos(pos.x, pos.y);

        /// <summary> Shows explosion at the given cell position. </summary>
        public void ShowExplosion(Vector2Int position, Action onComplete) => this.ShowExplosion(this.GetCellAtPos(position), onComplete);

        /// <summary> Shows explosion at the given cell. </summary>
        public void ShowExplosion(Cell cell, Action onComplete) => this.StartCoroutine(this.ShowExplosionCoroutine(cell, onComplete));

        /// <summary> Checks if the position is out of grid bounds. </summary>
        public bool IsOutOfBounds(Vector2Int position) => position.x < m_cells.GetLowerBound(0) || position.x > m_cells.GetUpperBound(0) || position.y < m_cells.GetLowerBound(1) || position.y > m_cells.GetUpperBound(1);

        /// <summary> Checks if the cell at the given position is empty. </summary>
        public bool IsEmptyAtPos(Vector2Int position)                     => !this.IsOutOfBounds(position) && this.GetCellAtPos(position).OccupyingUnit == null;

        Cell GetCellAtPos(int x, int y) => (Cell)this.m_cells.GetValue(x, y);
        Cell InitializeCellAtPos(int column, int row, float size)
        {
            var cell = Instantiate(cellPrefab, this.transform);
            cell.transform.localScale = Vector3.one * size;
            cell.transform.position = m_startPosition + new Vector3(column * size, 0, row * size);
            cell.Position = new Vector2Int(column + 1, row + 1);
            cell.CellSelected += OnCellSelected;
            m_cells.SetValue(cell, column + 1, row + 1);
            return cell;
        }

        void OnCellSelected(Cell cell) => this.CellSelected?.Invoke(cell);

        IEnumerator ShowExplosionCoroutine(Cell cell, Action onComplete)
        {
            this.explosion.transform.position = cell.transform.position;
            this.explosion.SetActive(true);
            yield return new WaitForSeconds(1);
            this.explosion.SetActive(false);
            onComplete?.Invoke();
        }
    }
}
