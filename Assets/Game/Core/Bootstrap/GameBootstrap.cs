using Core.StateMachine;
using UnityEngine;
using Zenject;

namespace Core.Bootstrap
{
    public class GameBootstrap : IInitializable
    {
        private readonly IGameStateMachine _stateMachine;

        public GameBootstrap(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async void Initialize()
        {
            await _stateMachine.Enter<BootstrapState>();
        }
    }
}
