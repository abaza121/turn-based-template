using System.Collections.Generic;
using TurnBased.Gameplay;

namespace TurnBased.Grids
{
    public class GridData
    {
        public int RowsCount { get; set;}
        public int ColumnsCount { get; set;}
        public float CellSize { get; set;}
        public List<Unit> CurrentUnits { get; set; }
    }
}
