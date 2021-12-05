using System.Collections.Generic;
using TurnBased.Gameplay;
using UnityEngine;

namespace TurnBased.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] HealthBar healthBarTemplate;

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
