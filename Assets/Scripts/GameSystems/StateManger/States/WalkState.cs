using System;
using GameSystem.Core;
using GameSystem.Core.ServiceLocator;
using GameSystem.StateManager;
using UnityEngine;

namespace GameSystem.StateManger
{
    public class WalkState : CharacterState
    {
        public Action OnWalkState;
        public override void Setup(CharacterStateManagerService managerService)
        {
            ManagerService = managerService;
            if (ManagerService.Animation == null)
            {
                throw new ArgumentNullException(nameof(ManagerService.Animation));
            }
        }

        public override void EnterState()
        {
            
            if (ManagerService==null)
            {
                Debug.LogError("the mangerService is Null");
            }
            else
            {
                if (ManagerService.Animation==null)
                {
                    Debug.LogError("the mangerService animation  is Null");    
                }
                else
                {
                    ManagerService.Animation.wrapMode=WrapMode.Loop;
                    ManagerService.Animation.CrossFade("Armature|walk");
                    ServiceLocator.Instance.Get<IAudioManagementService>().PlaySound(SoundEffect.Footsteps,true);
                }
                
            }
        }

        public override void UpdateState()
        {
        
        }

        public override void ExitState()
        {
            ServiceLocator.Instance.Get<IAudioManagementService>().StopLoopingSound();
            Reset();
        }

        public override void Reset()
        {
            ManagerService.FactoryService.ReleaseState(this);
        }
    }
}