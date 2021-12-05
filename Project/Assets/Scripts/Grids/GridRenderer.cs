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
        public event     Action<Cell> CellSelected;

        [SerializeField] Cell         cellPrefab;
        [SerializeField] GameObject   explosion;

        Array    m_cells;
        Vector3  m_startPosition;

        public void InitializeGridCells(GridData data)
        {
            this.m_cells         = Array.CreateInstance(typeof(Cell), new int[2] {data.ColumnsCount, data.RowsCount}, new int[2] { 1, 1 });
            this.m_startPosition = this.transform.position - (new Vector3((data.ColumnsCount * data.CellSize - data.CellSize), 0, (data.RowsCount * data.CellSize - data.CellSize)) * 0.5f);
            this.explosion.transform.GetChild(0).localScale = Vector3.one * 0.5f * data.CellSize;
            // Generate Cells.
            for(int i = 0;i< data.ColumnsCount; i++)
                for(int j = 0;j< data.RowsCount; j++)
                    InitializeCellAtPos(i, j, data.CellSize);

            // Populate Units.
            foreach(var unit in data.CurrentUnits)
            {
                var targetCell = (Cell)m_cells.GetValue(unit.CurrentCellPosition.x, unit.CurrentCellPosition.y);
                unit.transform.localScale = Vector3.one * data.CellSize;
                var targetLook = this.transform.position - targetCell.transform.position;
                unit.transform.SetPositionAndRotation(targetCell.transform.position, Quaternion.LookRotation(targetLook)); // the units initialize looking at the center point.
                targetCell.OccupyingUnit = unit;
            }
        }

        public void ShowCellsToChooseForUnits(IEnumerable<Unit> units, bool resetAll = true)
        {
            if(resetAll) ResetAll();
            foreach (var unit in units) GetCellAtPos(unit.CurrentCellPosition).ChangeHighlight(CellHighlightMode.CanChooseIt);
        }

        public void ResetAll()
        {
            for (int i = 1; i <= m_cells.GetUpperBound(0); i++)
                for (int j = 1; j <= m_cells.GetUpperBound(1); j++)
                    this.GetCellAtPos(i,j).ChangeHighlight(CellHighlightMode.NotHighlighted);
        }

        public void ShowActionsForUnit(Unit unit)
        {
            this.ResetAll();
            var attackRangeList = unit.CurrentCellPosition.GetPositionsAtRange(unit.AttackRange);
            foreach(var position in attackRangeList)
            {

                if (this.IsOutOfBounds(position)) continue;
                var targetCell = (Cell)this.m_cells.GetValue(position.x, position.y);
                if (targetCell.OccupyingUnit == null) continue;
                if (targetCell.OccupyingUnit.OwningPlayer != unit.OwningPlayer) targetCell.ChangeHighlight(CellHighlightMode.CanAttack);
            }

            var moveRangeList = unit.CurrentCellPosition.GetPositionsAtRange(unit.Energy);
            foreach (var position in moveRangeList)
            {

                if (this.IsOutOfBounds(position)) continue;
                var targetCell = (Cell)this.m_cells.GetValue(position.x, position.y);
                if (targetCell.OccupyingUnit == null) targetCell.ChangeHighlight(CellHighlightMode.CanMoveTo);
            }
        }

        public Cell GetCellAtPos(Vector2Int pos)                          => this.GetCellAtPos(pos.x, pos.y);
        public Cell GetCellAtPos(int x, int y)                            => (Cell)this.m_cells.GetValue(x, y);
        public void ShowExplosion(Vector2Int position, Action onComplete) => this.ShowExplosion(this.GetCellAtPos(position), onComplete);
        public void ShowExplosion(Cell cell, Action onComplete)           => this.StartCoroutine(this.ShowExplosionCoroutine(cell, onComplete));
        public bool IsOutOfBounds(Vector2Int position)                    => position.x < m_cells.GetLowerBound(0) || position.x > m_cells.GetUpperBound(0) || position.y < m_cells.GetLowerBound(1) || position.y > m_cells.GetUpperBound(1);
        public bool IsEmptyAtPos(Vector2Int position)                     => !this.IsOutOfBounds(position) && this.GetCellAtPos(position).OccupyingUnit == null;


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
