using System.Collections.Generic;
using UnityEngine;

namespace TurnBased.Utils
{
    /// <summary> Utilities that covers cell related maths.  </summary>
    public static class CellUtils
    {
        public static List<Vector2Int> GetPositionsAtRange(this Vector2Int position, Vector2Int range)
        {
            var resultList = new List<Vector2Int>();
            if (range.x == 0 && range.y == 0) return resultList;

            for(int i = position.x; i <= position.x + range.x; i++)
            {
                for (int j = position.y; j <= position.y + range.y; j++)
                    resultList.Add(new Vector2Int(i, j));
                for (int j = position.y - 1; j >= position.y - range.y; j--)
                    resultList.Add(new Vector2Int(i, j));
            }

            for (int i = position.x; i >= position.x - range.x; i--)
            {
                for (int j = position.y; j <= position.y + range.y; j++)
                    resultList.Add(new Vector2Int(i, j));
                for (int j = position.y - 1; j >= position.y - range.y; j--)
                    resultList.Add(new Vector2Int(i, j));
            }

            return resultList;
        }
    }
}
