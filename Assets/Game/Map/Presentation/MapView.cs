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

                newCell.transform.SetParent(mapParentContainerOfGrid.transform);
                newCell.AddComponent<MeshCollider>();

                Bounds bounds = newCell.GetComponent<Renderer>().bounds;
                float hexRadius = bounds.size.x / Mathf.Sqrt(3f);
                newCell.transform.position = HexToWorldPosition(hex.q, hex.r, hexRadius);

                newCell.GetComponent<HexView>().Bind(hex);

                cellsOfGrid.Add(hex, newCell);

                AnimateAppearCell(newCell.transform);

                await UniTask.Delay(50);
            }
        }

        private Vector3 HexToWorldPosition(int q, int r, float hexRadius)
        {
            float x = hexRadius * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            float z = hexRadius * (3f / 2f * r);
            return new Vector3(x, 0, z);
        }

        private float duration = 0.4f;
        private Ease easeType = Ease.OutBack;
        private void AnimateAppearCell(Transform newCell)
        {
            newCell.localScale = Vector3.zero;
            newCell.transform.DOScale(Vector3.one, duration).SetEase(easeType);
        }
    }
}
