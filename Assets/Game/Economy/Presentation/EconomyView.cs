using Economy.Domain;
using Economy.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Economy.Presentation
{
    public class EconomyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textIron;
        [SerializeField] private TextMeshProUGUI textWood;
        [SerializeField] private TextMeshProUGUI textFood;
        [SerializeField] private TextMeshProUGUI textEnergy;

        public void UpdateTexts(ResourceBase resourceBase)
        {
            textGold.text = $"{resourceBase.unitGold.value} / \n{resourceBase.unitGold.limit}";
            textIron.text = $"{resourceBase.unitIron.value} / \n{resourceBase.unitIron.limit}";
            textWood.text = $"{resourceBase.unitWood.value} / \n{resourceBase.unitWood.limit}";
            textFood.text = $"{resourceBase.unitFood.value} / \n{resourceBase.unitFood.limit}";

            float multiply = resourceBase.unitEnergy.value / 10;
            textEnergy.text = $"{1+multiply} X";
        }
    }
}
