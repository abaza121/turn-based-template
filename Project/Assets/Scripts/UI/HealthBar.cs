using TurnBased.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

namespace TurnBased.UI
{
    /// <summary>
    /// Shows the health for the given unit.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image foreground;
        [SerializeField] float positionOffset;
        [SerializeField] TextMeshProUGUI playerLabel;

        Unit m_unit;
        Camera m_camera;

        /// <summary>
        /// Initializes a bar for the given unit and grid scale.
        /// </summary>
        public void Initialize(Unit unit, float gridScale)
        {
            unit.HealthChanged += this.OnUnitHealthChanged;
            this.m_unit = unit;
            this.m_camera = Camera.main;
            this.positionOffset = gridScale * positionOffset;
            OnUnitHealthChanged();
            this.gameObject.SetActive(true);
            this.playerLabel.text = $"{unit.OwningPlayer + 1}";
        }

        void LateUpdate() => this.transform.position = this.m_camera.WorldToScreenPoint(m_unit.transform.position + Vector3.forward * positionOffset);  // Update the position of the bar based on the position of the unit and the offset.
        void OnUnitHealthChanged(Action onHealthEffectChanged = null)
        {
            DOTween.To(() => foreground.fillAmount, fillamount => foreground.fillAmount = fillamount, m_unit.HealthPercent, 1).OnComplete(() => OnHealthEffectComplete(onHealthEffectChanged));  // Show changing health effect, time should be less than explosion time.
        }

        void OnHealthEffectComplete(Action onHealthEffectComplete)
        {
            if (m_unit.HealthPercent <= 0) Destroy(this.gameObject);
            onHealthEffectComplete?.Invoke();
        }                     
    }
}
