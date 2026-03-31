using Map.Domain;
using UnityEngine;
using Map.Services;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Collections.Generic;
using DG.Tweening;

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

            foreach(Hex hex in gridData.hexagons)
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

            Bounds bounds = cell.GetComponent<Renderer>().bounds;
            float hexRadius = bounds.size.x / Mathf.Sqrt(3f);
            cell.transform.position = HexLayoutConverter.HexToWorldPosition(hex.q, hex.r, hexRadius);

            cell.GetComponent<HexView>().Bind(hex);

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
    }
}
