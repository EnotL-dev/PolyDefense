using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Map.Domain;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Map.Services
{
    public class HexSelectionService : IHexSelectionService, IInitializable, IDisposable
    {
        public Hex hoveredCell { get; private set; }
        private MeshRenderer hoveredMesh;
        public Hex selectedCell { get; private set; }
        private MeshRenderer selectedMesh;

        public event Action<Hex, Transform> OnHover;
        public event Action<Hex, Transform> OnSelect;
        public event Action OffSelect;

        private readonly SignalBus _signalBus;
        public HexSelectionService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<StateChangedSignal>(OnStateChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<StateChangedSignal>(OnStateChanged);
        }

        private bool gameStateBlock = false;
        private void OnStateChanged(StateChangedSignal state)
        {
            if (state.gameState is DayState)
                gameStateBlock = false;
            if (state.gameState is NightState)
                gameStateBlock = true;

            UnSelect();
            UnHover();
        }

        public void Hover(Hex hex, MeshRenderer mesh)
        {
            if (onUIBlock || gameStateBlock)
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
            if (onUIBlock || gameStateBlock || hex == selectedCell)
                return;

            UnSelect();
            selectedMesh = mesh;
            selectedMesh.renderingLayerMask = (1u << 0) | (1u << 1);

            selectedCell = hex;
            OnSelect?.Invoke(hex, mesh.transform);
        }

        public void UnSelect()
        {
            if (onUIBlock && !gameStateBlock)
                return;

            if (selectedMesh)
                selectedMesh.renderingLayerMask = 1u << 0;

            selectedCell = null;
            selectedMesh = null;

            OffSelect?.Invoke();
        }


        private bool onUIBlock = false; //Клик по UI игнорируем
        public async UniTask CheckEmptySpaceByClick()
        {
            while (true)
            {
                if (EventSystem.current && !Input.GetKeyDown(KeyCode.Escape))
                    onUIBlock = EventSystem.current.IsPointerOverGameObject(); //Клик по UI игнорируем

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
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

        public void RemoveUIBock() => onUIBlock = false; //После постройки
    }
}