using TurnBased.Data;
using UnityEngine;

namespace TurnBased.Grids
{
    /// <summary> Initializes and verifies grid data to be used by <see cref="Gameplay.GameplayManager"/> </summary>
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
            data.RowsCount = situation.GridSize.y;
            data.ColumnsCount = situation.GridSize.x;
            data.CellSize = situation.CellSize;
            return true;
        }

        public static void GenerateGridCells(GridData data, GridRenderer renderer) => renderer.InitializeGridCells(data);
    }
}
