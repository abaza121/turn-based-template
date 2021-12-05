using TurnBased.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TurnBased.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image foreground;
        [SerializeField] float positionOffset;

        Unit m_unit;
        Camera m_camera;

        public void Initialize(Unit unit, float size)
        {
            unit.HealthChanged += this.OnUnitHealthChanged;
            this.m_unit = unit;
            this.m_camera = Camera.main;
            this.positionOffset = size * positionOffset;
            OnUnitHealthChanged();
            this.gameObject.SetActive(true);
        }

        void LateUpdate() => this.transform.position = this.m_camera.WorldToScreenPoint(m_unit.transform.position + Vector3.forward * positionOffset);
        void OnUnitHealthChanged()
        {
            if(m_unit.HealthPercent <= 0) Destroy(this.gameObject);
            else                          DOTween.To(() => foreground.fillAmount, fillamount => foreground.fillAmount = fillamount, m_unit.HealthPercent, 1);
        }
    }
}
