using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.StateMachine
{
    public class DayState : IGameState
    {
        public UniTask Enter()
        {
            Debug.Log("Init Day");
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
