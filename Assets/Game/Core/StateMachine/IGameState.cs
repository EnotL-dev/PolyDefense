using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public interface IGameState
    {
        UniTask Enter();
        UniTask Exit();
    }
}
