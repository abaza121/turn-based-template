using System;
using UnityEngine;

namespace TurnBased.Data
{
    [CreateAssetMenu(menuName = "GameData/InitialSituation")]
    public class InitialSituation : ScriptableObject
    {
        public Vector2Int GridSize;
        public float CellSize;
        public UnitInitialState[] InitialUnits;
    }

    [Serializable]
    public struct UnitInitialState
    {
        public int OwningPlayer;
        public UnitType Type;
        public Vector2Int Position;
    }
}
