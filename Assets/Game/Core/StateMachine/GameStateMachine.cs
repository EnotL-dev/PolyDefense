using Zenject;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private IGameState _currentState;
        private readonly DiContainer _container;

        public GameStateMachine(DiContainer container)
        {
            _container = container;
        }

        public async UniTask Enter<TState>() where TState : IGameState
        {
            if (_currentState != null)
                await _currentState.Exit();

            _currentState = _container.Resolve<TState>();
            await _currentState.Enter();
        }
    }
}