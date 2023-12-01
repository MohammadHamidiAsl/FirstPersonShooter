using System;
using GameSystem.Factory;
using GameSystem.StateManger;
using UnityEngine;

namespace GameSystem.StateManager
{
    public class CharacterStateManagerService : StateManagerService
    {
        public CharacterState curentState { get; set; }

        // Reference to the character's animation controller
        public new Animation Animation { get; private set; }
        public StateFactoryServiceService FactoryService { get; private set; }

        public CharacterStateManagerService(Animation animation, StateFactoryServiceService stateFactoryService)
        {
            Animation = animation;
            FactoryService = stateFactoryService;
        }


        public void ChangeState(CharacterState newState)
        {
            if (newState == null)
            {
                Debug.LogError("New state is null.");
                return;
            }

            if (currentState is ReloadState && newState is not IdleState)
            {
                return;
            }

            newState.Setup(this);


            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public override void ChangeState(IState newState)
        {
        }

        public override void Update()
        {
            currentState?.UpdateState();

            // Here, you can add additional checks if needed
            // For example, to handle animation completion.
        }
    }
}