using Core.StateMachine;
using UnityEngine;
using Zenject;

namespace Core.Bootstrap
{
    public class GameBootstrap
    {
        private readonly IGameStateMachine _stateMachine;

        public GameBootstrap(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Start_Game()
        {
            Application.targetFrameRate = 60;
            _stateMachine.Enter<BootstrapState>();
        }
    }
}
