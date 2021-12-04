using System.Collections.Generic;
using TurnBased.Data;
using TurnBased.Grids;
using UnityEngine;

namespace TurnBased.Gameplay
{
    public static class UnitInitializer
    {
        public static bool GenerateUnits(InitialSituation situation, UnitsFactory unitsFactory, out List<Unit> units)
        {
            if (situation.InitialUnits.Length == 0)
            {
                units = null;
                Debug.LogError("No Units found for the game!");
                return false;
            }

            unitsFactory.InitializeFactory();
            units = new List<Unit>();
            foreach(var initUnit in situation.InitialUnits)
            {
                if (!unitsFactory.TryInitailizeUnitWithState(initUnit, out var unit))
                {
                    Debug.LogError("Failed to initialize unit with given state, some units will be missing.");
                    return false;
                }

                units.Add(unit);
            }

            return true;
        }
    }
}
