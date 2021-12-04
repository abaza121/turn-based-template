using System;
using System.Collections;
using System.Collections.Generic;
using TurnBased.Gameplay;
using TurnBased.Utils;
using UnityEngine;

namespace TurnBased.Grids
{
    public class GridRenderer : MonoBehaviour
    {
        public event Action<Cell> CellSelected;
        [SerializeField] Cell       cellPrefab;
        [SerializeField] GameObject explosion;
        Array    m_cells;
        Vector3  m_startPosition;

        public void InitializeGridCells(GridData data)
        {
            this.m_cells         = Array.CreateInstance(typeof(Cell), new int[2] {data.ColumnsCount, data.RowsCount}, new int[2] { 1, 1 });
            this.m_startPosition = this.transform.position - (new Vector3((data.ColumnsCount * data.CellSize - data.CellSize), 0, (data.RowsCount * data.CellSize - data.CellSize)) * 0.5f);

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
                unit.transform.SetPositionAndRotation(targetCell.transform.position, Quaternion.LookRotation(targetLook)); // The rotation assumes there are only 2 players, will need to be updated if needed more custom behavior.
                targetCell.OccupyingUnit = unit;
            }
        }

        Cell InitializeCellAtPos(int column, int row, float size)
        {
            var cell = Instantiate(cellPrefab, this.transform);
            cell.transform.localScale = Vector3.one * size;
            cell.transform.position = m_startPosition + new Vector3(column * size, 0, row * size);
            cell.Position = new Vector2Int(column + 1, row + 1);
            cell.CellSelected += OnCellSelected;
            cell.CellAttacked += OnCellAttacked;
            m_cells.SetValue(cell, column + 1, row + 1);
            return cell;
        }

        public void ShowCellsToChooseForUnits(List<Unit> units, bool resetAll = true)
        {
            if(resetAll) ResetAll();
            foreach (var unit in units)
            {
                ((Cell)m_cells.GetValue(unit.CurrentCellPosition.x, unit.CurrentCellPosition.y)).ChangeHighlight(CellHighlightMode.CanChooseIt);
            }    
        }

        public void ResetAll()
        {
            for (int i = 1; i <= m_cells.GetUpperBound(1); i++)
                for (int j = 1; j <= m_cells.GetUpperBound(1); j++)
                    ((Cell)m_cells.GetValue(i, j)).ChangeHighlight(CellHighlightMode.NotHighlighted);
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

            var moveRangeList = unit.CurrentCellPosition.GetPositionsAtRange(unit.MoveRange);
            foreach (var position in moveRangeList)
            {

                if (this.IsOutOfBounds(position)) continue;
                var targetCell = (Cell)this.m_cells.GetValue(position.x, position.y);
                if (targetCell.OccupyingUnit == null) targetCell.ChangeHighlight(CellHighlightMode.CanMoveTo);
            }
        }

        void OnCellSelected(Cell cell) => this.CellSelected?.Invoke(cell);
        void OnCellAttacked(Cell cell) => this.StartCoroutine(this.ShowExplosion(cell));

        IEnumerator ShowExplosion(Cell cell)
        {
            this.explosion.transform.position = cell.transform.position;
            this.explosion.SetActive(true);
            yield return new WaitForSeconds(1);
            this.explosion.SetActive(false);
        }

        bool IsOutOfBounds(Vector2Int position) => position.x < m_cells.GetLowerBound(0) || position.x > m_cells.GetUpperBound(0) || position.y < m_cells.GetLowerBound(1) || position.y > m_cells.GetUpperBound(1);
    }
}
