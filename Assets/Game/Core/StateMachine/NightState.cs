using Combat.Services;
using Cysharp.Threading.Tasks;
using Economy.Services;
using Map.Services;
using Zenject;

namespace Core.StateMachine
{
    public class NightState : IGameState
    {
        [Inject] ICombatService combatService;
        [Inject] IEconomyService economyService;
        [Inject] IMapService mapService;

        public UniTask Enter()
        {
            combatService.StartWawe();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            economyService.IncomeCycle(mapService.CurrentMap);
            return UniTask.CompletedTask;
        }
    }
}
