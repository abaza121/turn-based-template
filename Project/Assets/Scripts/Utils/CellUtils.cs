using System.Collections;
using System.Collections.Generic;
using TurnBased.Gameplay;
using UnityEngine;

namespace TurnBased.Utils
{
    public static class CellUtils
    {
        public static List<Vector2Int> GetPositionsAtRange(this Vector2Int position, Vector2Int range)
        {
            var resultList = new List<Vector2Int>();
            if (range.x == 0 && range.y == 0) return resultList;

            for(int i = position.x; i <= position.x + range.x; i++) resultList.Add(new Vector2Int(i, position.y));
            for(int i = position.x; i >= position.x - range.x; i--) resultList.Add(new Vector2Int(i, position.y));
            for(int i = position.y; i <= position.y + range.y; i++) resultList.Add(new Vector2Int(position.x, i));
            for(int i = position.y; i >= position.y - range.y; i--) resultList.Add(new Vector2Int(position.x, i));
            return resultList;
        }
    }
}
