using System.Collections.Generic;
using TurnBased.Gameplay;
using UnityEngine;

namespace TurnBased.UI
{
    /// <summary>
    /// Controls shown health bars and initializes it.
    /// </summary>
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] HealthBar healthBarTemplate;

        /// <summary>
        /// Initialize health bars for the given unit.
        /// </summary>
        public void InitializeHealthBarsForUnits(List<Unit> units, float size)
        {
            this.transform.localScale = Vector3.one * size;
            foreach (var unit in units)
            {
                var healthbar = GameObject.Instantiate(healthBarTemplate, this.transform);
                healthbar.Initialize(unit, size);
            }
        }
    }
}
