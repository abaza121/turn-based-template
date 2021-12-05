using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TurnBased.UI
{
    public class SkipButton : MonoBehaviour
    {
        [SerializeField] private Button          skipButton;
        [SerializeField] private TextMeshProUGUI skipText;

        public void ChangeText(string text)                => skipText.text = text;
        public void ChangeButtonState(bool isInteractable) => skipButton.interactable = isInteractable;
    }
}
