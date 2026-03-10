using Cysharp.Threading.Tasks;
using Map.Presentation;
using Map.Services;
using UnityEngine;
using Zenject;

namespace Core.StateMachine
{
    public class BootstrapState : IGameState
    {
        private readonly IGameStateMachine stateMachine;
        private readonly IMapService mapService;

        [Inject] private MapView mapView;
        [Inject] IHexSelectionService hexSelectionService;

        public BootstrapState(IGameStateMachine stateMachine, IMapService mapService)
        {
            this.stateMachine = stateMachine;
            this.mapService = mapService;
        }

        public async UniTask Enter()
        {
            mapService.GenerateMap(4);

            await stateMachine.Enter<DayState>();
        }

        public async UniTask Exit()
        {
            hexSelectionService.CheckEmptySpaceByClick().Forget();
            await mapView.Render();
        }
    }
}