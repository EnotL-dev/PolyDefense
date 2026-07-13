using Cysharp.Threading.Tasks;
using Map.Services;
using Zenject;

namespace Core.StateMachine
{
    public class NightState : IGameState
    {
        [Inject] IMapService mapService;

        public UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
