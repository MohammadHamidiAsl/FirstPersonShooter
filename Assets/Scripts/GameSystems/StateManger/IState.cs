using GameSystem.Core;


namespace GameSystem.StateManager
{
    public interface IState:IPoolable
    {

        
        void EnterState();
        void UpdateState();
        void ExitState();
    }
}
