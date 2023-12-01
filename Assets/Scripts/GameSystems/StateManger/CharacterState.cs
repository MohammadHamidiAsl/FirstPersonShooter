using GameSystem.StateManager;
using GameSystem.Core;

namespace GameSystem.StateManger
{
    public abstract class CharacterState : IState
    {
        public CharacterStateManagerService ManagerService { get; set; }


        public abstract void Reset();

        public abstract void Setup(CharacterStateManagerService managerService);

        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void ExitState();
    }
}