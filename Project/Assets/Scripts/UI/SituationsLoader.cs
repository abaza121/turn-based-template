using TurnBased.Data;
using UnityEngine;

namespace TurnBased.UI
{
    public class SituationsLoader : MonoBehaviour
    {
        [SerializeField] SituationItemUI    situationTemplate;
        [SerializeField] InitialSituation[] availableSituations;
        [SerializeField] Transform          situationItemsParent;

        private void Start()
        {
            for(int i = 0; i < availableSituations.Length; i++)
            {
                var situationUI = GameObject.Instantiate(situationTemplate, situationItemsParent);
                situationUI.Initialize(availableSituations[i], i == 0);
                situationUI.gameObject.SetActive(true);
            }
        }
    }
}
