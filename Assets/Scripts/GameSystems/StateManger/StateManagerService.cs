using UnityEngine;

namespace GameSystem.StateManager
{
    
    public abstract class StateManagerService : IStateManagerService
    {
        public IState currentState { get; set; }
    

        public Animation Animation { get; set; }

        public abstract void ChangeState(IState newState);
        
        public virtual void Update()
        {
            
        }

    }
}