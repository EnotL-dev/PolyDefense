using Cysharp.Threading.Tasks;


namespace Core.StateMachine
{
    public class WinState : IGameState
    {
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
