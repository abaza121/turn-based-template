using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TurnBased.Data;

namespace TurnBased.UI
{
    public class SituationItemUI : MonoBehaviour
    {
        [SerializeField] MainMenuUIManager mainMenuUIManager;
        [SerializeField] TextMeshProUGUI toggleText;
        [SerializeField] Toggle toggle;

        InitialSituation m_situation;

        public void Initialize(InitialSituation situation, bool initState)
        {
            toggleText.text = situation.name;
            this.m_situation = situation;
            this.toggle.onValueChanged.AddListener(this.OnToggleStateChanged);
            this.toggle.isOn = initState;
            if (initState) OnToggleStateChanged(true);
        }

        void OnToggleStateChanged(bool state)
        {
            if(state) this.mainMenuUIManager.ChosenSituationChanged(m_situation);
        }
    }
}
