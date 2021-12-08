using System;
using TurnBased.Player;
using UnityEngine;

namespace TurnBased.Data
{
    /// <summary>
    /// A full game initial state contains different data that is used by the game to initialize first turn.
    /// </summary>
    [CreateAssetMenu(menuName = "GameData/InitialSituation")]
    public class InitialSituation : ScriptableObject
    {
        /// <summary>
        /// The size of the grid in cells count per axis.
        /// </summary>
        public Vector2Int GridSize;

        /// <summary>
        /// The Cell scale modifiers, this is to insure a static position in front of static camera.
        /// </summary>
        public float CellSize;

        /// <summary>
        /// The players available at the start of the situation.
        /// </summary>
        public PlayerType[] Players;

        /// <summary>
        /// Units, what player owns it and where it is.
        /// </summary>
        public UnitInitialState[] InitialUnits;
    }

    /// <summary>
    /// Contains the data needed for a unit in initial situation.
    /// </summary>
    [Serializable]
    public struct UnitInitialState
    {
        public int OwningPlayer;
        public UnitType Type;
        public Vector2Int Position;
    }
}
