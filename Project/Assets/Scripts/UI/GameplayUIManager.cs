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
        /// <summary> Invoked when the skip button is pressed. </summary>
        public event Action SkipButtonPressed;

        [SerializeField] HealthBarController healthBarController;
        [SerializeField] SkipButton          skipButton;
        [SerializeField] EndGamePanel        endGamePanel;

        /// <summary> Initialize the healthbars for the given units and scale it based on the given gridScale </summary>
        public void InitializeHealthbars(List<Unit> units, float gridScale) => this.healthBarController.InitializeHealthBarsForUnits(units, gridScale);

        /// <summary> Invokes Skip Button Pressed event, used as serialized action in unity. </summary>
        public void OnSkipButtonPressed()                              => SkipButtonPressed?.Invoke();

        /// <summary> Switchs UI state to ready to end. </summary>
        public void ReadyToEndTurn()                                   => skipButton.ChangeText("End Turn");

        /// <summary> Change the interactability of the skip button. </summary>
        public void ChangeSkipButtonState(bool state)                  => skipButton.ChangeButtonState(state);

        /// <summary> Shows end game panel with the given winning player index. </summary>
        public void ShowEndGamePanel(int playerId)                     => endGamePanel.ShowEndGamePanel(playerId);

        /// <summary> Switch UI state to be be at the start of a turn. </summary>
        public void TurnStarted(PlayerType type)
        {
            skipButton.ChangeText("Skip Turn");
            skipButton.ChangeButtonState(type == PlayerType.Human);
        }
    }
}
