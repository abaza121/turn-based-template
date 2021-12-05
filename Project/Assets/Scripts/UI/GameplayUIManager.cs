using System;
using System.Collections.Generic;
using TurnBased.Gameplay;
using TurnBased.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBased.UI
{
    /// <summary>
    /// Responsible for handling gameplay UI Logic and events.
    /// </summary>
    public class GameplayUIManager : MonoBehaviour
    {
        public event Action SkipButtonPressed;

        [SerializeField] HealthBarController healthBarController;
        [SerializeField] SkipButton          skipButton;
        [SerializeField] GameObject          endGamePanel;
        public void InitializeHealthbars(List<Unit> units, float size) => this.healthBarController.InitializeHealthBarsForUnits(units, size);
        public void OnSkipButtonPressed()                              => SkipButtonPressed?.Invoke();
        public void ReadyToEndTurn()                                   => skipButton.ChangeText("End Turn");
        public void ChangeSkipButtonState(bool state)                  => skipButton.ChangeButtonState(state);
        public void ShowEndGamePanel(int playerId)                     => endGamePanel.SetActive(true);
        public void TurnStarted(PlayerType type)
        {
            skipButton.ChangeText("Skip Turn");
            skipButton.ChangeButtonState(type == PlayerType.Hooman);
        }
    }
}
