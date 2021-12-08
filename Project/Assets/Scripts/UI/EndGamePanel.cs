using UnityEngine;
using TMPro;

namespace TurnBased.UI
{
    /// <summary>
    /// The game over panel in which the player can restart with same situation or try a different one.
    /// </summary>
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
