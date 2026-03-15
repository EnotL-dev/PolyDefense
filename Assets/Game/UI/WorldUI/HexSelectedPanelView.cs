using DG.Tweening;
using Economy.Services;
using Map.Domain;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.WorldUI
{
    public class HexSelectedPanelView : MonoBehaviour
    {
        [Inject] IEconomyService economyService;

        private CanvasGroup canvasGroup;

        [SerializeField] private GameObject panelHex;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textNameBuilding;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private GridLayoutGroup panelGain;
        [SerializeField] private GridLayoutGroup panelCost;
        [Space(5)]
        [SerializeField] private GameObject panelBuild;

        [Space(5)]
        [SerializeField] private ResourcePanelView resourcePanelPrefab;
        private List<ResourcePanelView> resourcePanelsGainPool = new List<ResourcePanelView>();
        private List<ResourcePanelView> resourcePanelsCostPool = new List<ResourcePanelView>();

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowPanel(Hex hex, Vector3 pos)
        {
            transform.position = pos;
            transform.forward = transform.position - Camera.main.transform.position;

            ClearPools();
            canvasGroup.DOKill(complete: false);
            canvasGroup.transform.DOKill(complete: false);

            canvasGroup.transform.localScale = Vector3.zero;
            canvasGroup.transform.DOScale(0.002f, 0.5f);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.2f);

            SetTexts(hex);
            SetNewResourcePanels(hex);

            if (hex.biome == BiomeType.Basic)
                panelHex.SetActive(true);
        }

        public void HidePanel()
        {
            canvasGroup.transform.DOScale(0f, 0.15f);
            canvasGroup.DOFade(0, 0.15f).OnComplete(ClearPools);
        }

        private void ClearPools()
        {
            foreach(ResourcePanelView rpv in resourcePanelsGainPool)
            {
                Destroy(rpv.gameObject);
            }
            foreach (ResourcePanelView rpv in resourcePanelsCostPool)
            {
                Destroy(rpv.gameObject);
            }

            resourcePanelsGainPool.Clear();
            resourcePanelsCostPool.Clear();
        }

        private void SetTexts(Hex hex)
        {
            textName.text = HexDescription.GetBiomeName(hex.biome);
        }

        private void SetNewResourcePanels(Hex hex)
        {

        }

        public void TryBuild()
        {
            
        }
    }
}
