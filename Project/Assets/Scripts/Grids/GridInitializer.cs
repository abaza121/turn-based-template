using TurnBased.Data;
using UnityEngine;

namespace TurnBased.Grids
{
    public static class GridInitializer
    {
        public static bool TryGenerateGridData(InitialSituation situation, out GridData data)
        {
            if (situation.GridSize.x == 0 || situation.GridSize.y == 0 || situation.CellSize == 0)
            {
                data = null;
                Debug.LogError("Invalid grid size or cell size");
                return false;
            }

            data = new GridData();
            data.RowsCount = situation.GridSize.x;
            data.ColumnsCount = situation.GridSize.y;
            data.CellSize = situation.CellSize;
            return true;
        }

        public static void GenerateGridCells(GridData data, GridRenderer renderer) => renderer.InitializeGridCells(data);
    }
}
