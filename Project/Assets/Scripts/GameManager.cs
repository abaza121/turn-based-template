using TurnBased.Data;
using TurnBased.Utils;
using UnityEngine.SceneManagement;

namespace TurnBased
{
    /// <summary>
    /// Responsible for controlling data for gameplay and scene change handling.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// The current chosen situation.
        /// </summary>
        public InitialSituation ChosenInitalSituation { get; set; }

        public void GoToGameplayScene() => SceneManager.LoadScene("Game");
        public void GoToMainMenu()      => SceneManager.LoadScene("Menu");
    }
}
