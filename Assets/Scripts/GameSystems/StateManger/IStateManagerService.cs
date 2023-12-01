using UnityEngine;

namespace GameSystem.StateManager
{
    public interface IStateManagerService
    {
        public Animation Animation { get; set; }
        void ChangeState(IState newState);
        void Update();
    }
}

