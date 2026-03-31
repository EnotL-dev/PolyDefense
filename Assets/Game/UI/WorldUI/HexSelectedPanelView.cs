using Construction.Config;
using Construction.Services;
using DG.Tweening;
using Economy.Config;
using Economy.Domain;
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
        [Inject] IBuildService buildService;
        [Inject] IEconomyService economyService;

        private CanvasGroup canvasGroup;

        [SerializeField] private GameObject panelHex;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textNameBuilding;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private GridLayoutGroup panelGain;
        [SerializeField] private GridLayoutGroup panelCost;

        [Space(5)]
        [SerializeField] private ResourcePanelView resourcePanelPrefab;
        private List<ResourcePanelView> resourcePanelsGainPool = new List<ResourcePanelView>();
        private List<ResourcePanelView> resourcePanelsCostPool = new List<ResourcePanelView>();

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private Hex hex = null;
        public void ShowPanel(Hex hex, Vector3 pos)
        {
            this.hex = hex;

            //transform.position = pos; // Над клеткой
            Vector3 direction = Camera.main.transform.forward; // Направление камеры
            transform.position = Camera.main.transform.position + direction * 3f;
            transform.forward = transform.position - Camera.main.transform.position;

            ClearPools();
            canvasGroup.DOKill(complete: false);
            canvasGroup.transform.DOKill(complete: false);

            canvasGroup.transform.localScale = Vector3.zero;
            canvasGroup.transform.DOScale(0.002f, 0.5f);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.2f);

            SetNewResourcePanels();
            SetTexts();

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

        private void SetTexts()
        {
            if (hex.building == null)
            {
                textName.text = HexDescription.GetBiomeName(hex.biome);
                textNameBuilding.text = buildingsDataBase.buildings.Find(x => x.biome == hex.biome).title;
            }
            else
            {
                textName.text = hex.building.title;
            }
        }

        BuildingsDataBase buildingsDataBase = null;
        ResourceIconBase resourceIconBase = null;
        private void SetNewResourcePanels()
        {
            if(!buildingsDataBase)
                buildingsDataBase = Resources.Load<BuildingsDataBase>("Building/BuildingsDataBase");
            if(!resourceIconBase)
                resourceIconBase = Resources.Load<ResourceIconBase>("Economy/ResourceIconBase");

            foreach (Building building in buildingsDataBase.buildings) //Ищем пастройку для данного биома
            {
                if(building.biome == hex.biome)
                {
                    if(building.resourcesCost != null)
                    {
                        foreach(ResourceUnit resourceUnit in building.resourcesCost)
                        {
                            ResourcePanelView newPanel = Instantiate(resourcePanelPrefab);

                            string addColor = "";
                            if (economyService.CheckSum(resourceUnit))
                                addColor += "<color=green>";
                            else
                                addColor += "<color=red>";

                            string textRes = $"{addColor}{resourceUnit.value}</color>";
                            Sprite spriteRes = resourceIconBase.iconsDictionary.Find(x => x.resourceUnit.GetType() == resourceUnit.GetType()).icon;

                            newPanel.Init(spriteRes, textRes);
                            newPanel.transform.SetParent(panelCost.transform, false);
                            resourcePanelsCostPool.Add(newPanel);
                        }
                    }
                    if (building.resourcesIncome != null)
                    {
                        foreach (ResourceUnit resourceUnit in building.resourcesIncome)
                        {
                            ResourcePanelView newPanel = Instantiate(resourcePanelPrefab);

                            string textRes = $"+ {resourceUnit.value}";
                            Sprite spriteRes = resourceIconBase.iconsDictionary.Find(x => x.resourceUnit.GetType() == resourceUnit.GetType()).icon;

                            newPanel.Init(spriteRes, textRes);
                            newPanel.transform.SetParent(panelGain.transform, false);
                            resourcePanelsGainPool.Add(newPanel);
                        }
                    }
                    if (building.resourcesAddLimit != null)
                    {
                        foreach (ResourceUnit resourceUnit in building.resourcesAddLimit)
                        {
                            ResourcePanelView newPanel = Instantiate(resourcePanelPrefab);

                            string textRes = $"<color=blue>{resourceUnit.limit} to limit</color>";
                            Sprite spriteRes = resourceIconBase.iconsDictionary.Find(x => x.resourceUnit.GetType() == resourceUnit.GetType()).icon;

                            newPanel.Init(spriteRes, textRes);
                            newPanel.transform.SetParent(panelGain.transform, false);
                            resourcePanelsGainPool.Add(newPanel);
                        }
                    }

                    break;
                }
            }
        }

        public void Build() //Из кнопки вызов
        {
            Building building = buildingsDataBase.buildings.Find(x => x.biome == hex.biome);
            if (buildService.CheckBuild(building))
            {
                buildService.Build(building, hex);
                HidePanel();
            }
        }
    }
}
