using Cysharp.Threading.Tasks;
using Map.Domain;
using System;
using UnityEngine;

namespace Map.Services
{
    public class HexSelectionService : IHexSelectionService
    {
        public Hex hoveredCell { get; private set; }
        private MeshRenderer hoveredMesh;
        public Hex selectedCell { get; private set; }
        private MeshRenderer selectedMesh;

        public event Action<Hex> OnHover;
        public event Action<Hex> OnSelect;

        public void Hover(Hex hex, MeshRenderer mesh)
        {
            UnHover();
            hoveredMesh = mesh;
            hoveredMesh.renderingLayerMask = (1u << 0) | (1u << 1);

            hoveredCell = hex;
            OnHover?.Invoke(hex);
        }

        public void UnHover()
        {
            if (hoveredMesh && hoveredMesh != selectedMesh)
                hoveredMesh.renderingLayerMask = 1u << 0;

            hoveredCell = null;
            hoveredMesh = null;
        }

        public void Select(Hex hex, MeshRenderer mesh)
        {
            UnSelect();
            selectedMesh = mesh;
            selectedMesh.renderingLayerMask = (1u << 0) | (1u << 1);

            selectedCell = hex;
            OnSelect?.Invoke(hex);

            Debug.Log("Select");
        }

        public void UnSelect()
        {
            if (selectedMesh)
                selectedMesh.renderingLayerMask = 1u << 0;

            selectedCell = null;
            selectedMesh = null;

            Debug.Log("UnSelect");
        }

        public async UniTask CheckEmptySpaceByClick()
        {
            Debug.Log("Start checking empty space for unselecting cells");

            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (!Physics.Raycast(ray))
                    {
                        UnSelect();
                    }
                }

                await UniTask.Yield();
            }
        }
    }
}