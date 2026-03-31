using Cysharp.Threading.Tasks;
using Map.Domain;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map.Services
{
    public class HexSelectionService : IHexSelectionService
    {
        public Hex hoveredCell { get; private set; }
        private MeshRenderer hoveredMesh;
        public Hex selectedCell { get; private set; }
        private MeshRenderer selectedMesh;

        public event Action<Hex, Transform> OnHover;
        public event Action OffHover;
        public event Action<Hex, Transform> OnSelect;
        public event Action OffSelect;

        public void Hover(Hex hex, MeshRenderer mesh)
        {
            if (onUIBlock)
                return;

            UnHover();
            hoveredMesh = mesh;
            hoveredMesh.renderingLayerMask = (1u << 0) | (1u << 1);

            hoveredCell = hex;
            OnHover?.Invoke(hex, mesh.transform);
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
            if (onUIBlock)
                return;

            UnSelect();
            selectedMesh = mesh;
            selectedMesh.renderingLayerMask = (1u << 0) | (1u << 1);

            selectedCell = hex;
            OnSelect?.Invoke(hex, mesh.transform);

            Debug.Log("Select");
        }

        public void UnSelect()
        {
            if (onUIBlock)
                return;

            if (selectedMesh)
                selectedMesh.renderingLayerMask = 1u << 0;

            selectedCell = null;
            selectedMesh = null;

            OffSelect?.Invoke();

            Debug.Log("UnSelect");
        }


        private bool onUIBlock = false; //Клик по UI игнорируем
        public async UniTask CheckEmptySpaceByClick()
        {
            Debug.Log("Start checking empty space for unselecting cells");

            while (true)
            {
                if(EventSystem.current)
                    onUIBlock = EventSystem.current.IsPointerOverGameObject(); //Клик по UI игнорируем

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