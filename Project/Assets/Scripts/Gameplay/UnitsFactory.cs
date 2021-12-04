using System.Collections.Generic;
using TurnBased.Data;
using UnityEngine;

namespace TurnBased.Gameplay
{
    public class UnitsFactory : MonoBehaviour
    {
        [SerializeField] private UnitSetup[] currentSetups;
        Dictionary<UnitType, UnitSetup> m_unitPrefabs;

        public bool InitializeFactory()
        {
            if (currentSetups.Length == 0)
            {
                Debug.LogError("No Unit Setups found");
                return false;
            }

            this.m_unitPrefabs = new Dictionary<UnitType, UnitSetup>();

            foreach(var setup in currentSetups)
            {
                if(this.m_unitPrefabs.ContainsKey(setup.Type))
                {
                    Debug.LogError("Found duplicate setup of the same type");
                    return false;
                }

                this.m_unitPrefabs[setup.Type] = setup;
            }

            return true;
        }

        public bool TryInitailizeUnitWithState(UnitInitialState state, out Unit unit)
        {
            if(!this.m_unitPrefabs.TryGetValue(state.Type, out var setup))
            {
                unit = null;
                Debug.LogError($"failed to find unit for type {state.Type}");
                return false;
            }

            unit = GameObject.Instantiate(setup.Prefab, this.transform);
            unit.SetInitialState(setup);
            unit.OwningPlayer = state.OwningPlayer;
            unit.CurrentCellPosition = state.Position;
            return true;
        }
    }
}
