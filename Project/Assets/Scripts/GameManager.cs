using TurnBased.Data;
using TurnBased.Utils;
using UnityEngine.SceneManagement;

namespace TurnBased
{
    public class GameManager : Singleton<GameManager>
    {
        public InitialSituation ChosenInitalSituation { get; set; }

        public void GoToGameplayScene() => SceneManager.LoadScene("Game");
        public void GoToMainMenu()      => SceneManager.LoadScene("Menu");
    }
}
