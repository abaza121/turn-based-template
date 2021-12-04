using TurnBased.Gameplay;
using UnityEngine;

namespace TurnBased.Data
{
    [CreateAssetMenu(menuName = "GameData/UnitSetup")]
    public class UnitSetup : ScriptableObject
    {
        public Unit Prefab;
        public UnitType Type;
        public string Name;
        public int Health;
        public int Damage;
        public Vector2Int AttackRange;
        public Vector2Int MoveRange;
    }
}
