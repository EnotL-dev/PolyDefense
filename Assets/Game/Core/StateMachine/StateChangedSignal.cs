using UnityEngine;

namespace Core.StateMachine
{
    public class StateChangedSignal
    {
        public IGameState gameState { get; }

        public StateChangedSignal(IGameState gameState)
        {
            this.gameState = gameState;
        }
    }
}
