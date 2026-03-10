using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public interface IGameStateMachine
    {
        UniTask Enter<TState>() where TState : IGameState;
    }
}
