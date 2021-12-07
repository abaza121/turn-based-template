using System.Collections;
using System.Collections.Generic;
using TurnBased.Data;
using UnityEngine;

namespace TurnBased.UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        public void ChosenSituationChanged(InitialSituation initialSituation) => GameManager.Instance.ChosenInitalSituation = initialSituation;

        public void OnStartGameButtonPressed() => GameManager.Instance.GoToGameplayScene();
    }
}
