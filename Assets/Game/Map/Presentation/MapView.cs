using Map.Domain;
using UnityEngine;
using Map.Services;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Collections.Generic;
using DG.Tweening;
using Construction.Presentation;

namespace Map.Presentation
{
    public class MapView : MonoBehaviour
    {
        [Inject] IMapService mapService;
        [Inject] DiContainer container;
        GridData gridData => mapService.CurrentMap;

        [SerializeField] private HexConfig hexConfig;

        private Transform mapParentContainerOfGrid;
        private Dictionary<Hex, GameObject> cellsOfGrid = new Dictionary<Hex, GameObject>();

        public async UniTask Render()
        {
            Debug.Log("<color=green>Map is rendering</color>");

            if (!mapParentContainerOfGrid)
            {
                mapParentContainerOfGrid = new GameObject().transform;
                mapParentContainerOfGrid.name = "Grid";
            }

            foreach (Hex hex in gridData.hexagons)
            {
                HexCellData currentHexData = hexConfig.hexCellDatas.Find(curHex => curHex.biome == hex.biome);
                if (currentHexData == null) { Debug.Log("Cell not found!"); continue; }

                GameObject newCell = container.InstantiatePrefab(currentHexData.hexProp);

                CreateNewCell(hex, newCell);

                await UniTask.Delay(50);
            }
        }

        private void CreateNewCell(Hex hex, GameObject cell)
        {
            cell.transform.SetParent(mapParentContainerOfGrid.transform);
            cell.AddComponent<MeshCollider>();

            if (mapService.HexSize == 0)
            {
                Bounds bounds = cell.GetComponent<Renderer>().bounds;
                float hexRadius = bounds.size.x / Mathf.Sqrt(3f);
                mapService.SetHexSize(hexRadius);
            }
            cell.transform.position = HexLayoutConverter.HexToWorldPosition(hex.q, hex.r, mapService.HexSize);

            cell.GetComponent<HexView>().Bind(hex);
            if (hex.building && hex.building.defenseConfig)
            {
                DefenseBuildingView defenseView = cell.AddComponent<DefenseBuildingView>();
                container.Inject(defenseView);
                defenseView.Initialize(hex);
            }

            cellsOfGrid.Add(hex, cell);

            AnimateAppearCell(cell.transform);
        }

        private float duration = 0.4f;
        private Ease easeType = Ease.OutBack;
        private void AnimateAppearCell(Transform newCell)
        {
            newCell.localScale = Vector3.zero;
            newCell.transform.DOScale(Vector3.one, duration).SetEase(easeType);
        }

        public void ChangeCell(Hex hex, GameObject cell)
        {
            Destroy(cellsOfGrid[hex]);
            cellsOfGrid.Remove(hex);

            GameObject newCell = container.InstantiatePrefab(cell);
            CreateNewCell(hex, newCell);
        }

        //EnemyView גûחûגאוע
        public void ChangeCellByBiome(Hex hex)
        {
            Destroy(cellsOfGrid[hex]);
            cellsOfGrid.Remove(hex);

            HexCellData currentHexData = hexConfig.hexCellDatas.Find(curHex => curHex.biome == hex.biome);
            if (currentHexData == null) { Debug.Log("Cell not found!"); return; }

            GameObject newCell = container.InstantiatePrefab(currentHexData.hexProp);
            CreateNewCell(hex, newCell);
        }
    }
}