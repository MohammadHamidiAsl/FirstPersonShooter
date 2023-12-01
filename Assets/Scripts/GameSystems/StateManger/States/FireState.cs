using System;
using GameSystem.Core;
using GameSystem.Core.ServiceLocator;
using GameSystem.StateManager;
using UnityEngine;

namespace GameSystem.StateManger
{
    public class FireState : CharacterState
    {

        public Action OnFire;
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
                    ManagerService.Animation.wrapMode=WrapMode.Once;
                    
                    ManagerService.Animation.Play("Armature|fire");
                    ServiceLocator.Instance.Get<IAudioManagementService>().PlaySound(SoundEffect.Fire,false);
                    
                }
                
            }
        }

        public override void UpdateState()
        {
            
            if (!Input.GetButton("Fire1"))
            {
                
                ManagerService.ChangeState(ManagerService.FactoryService.CreateCharacterState<IdleState>());
            }
        }

        public override void ExitState()
        {
            Reset();
        }

        public override void Reset()
        {
            
        }
    }
}