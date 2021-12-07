using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TurnBased.UI
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI descriptionLabel;
        public void ShowEndGamePanel(int winningPlayerId)
        {
            this.descriptionLabel.text = $"Player {winningPlayerId} won.";
            this.gameObject.SetActive(true);
        }

        public void OnRestartButtonPressed() => GameManager.Instance.GoToGameplayScene();
        public void OnMenuButtonPressed()    => GameManager.Instance.GoToMainMenu();
    }
}
