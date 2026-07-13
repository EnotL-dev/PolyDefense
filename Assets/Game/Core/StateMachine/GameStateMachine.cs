using Zenject;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private IGameState _currentState;
        private readonly DiContainer _container;
        private readonly SignalBus _signalBus;

        public GameStateMachine(DiContainer container, SignalBus signalBus)
        {
            _container = container;
            _signalBus = signalBus;
        }

        public async UniTask Enter<TState>() where TState : IGameState
        {
            if (_currentState?.GetType() == typeof(TState))
                return;

            if (_currentState != null)
                await _currentState.Exit();

            _currentState = _container.Resolve<TState>();
            await _currentState.Enter();

            _signalBus.Fire(new StateChangedSignal(_currentState));
        }
    }
}