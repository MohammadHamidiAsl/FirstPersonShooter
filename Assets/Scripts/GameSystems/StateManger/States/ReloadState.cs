using System;
using GameSystem.Core;
using GameSystem.Core.ServiceLocator;
using GameSystem.StateManager;
using UnityEngine;

namespace GameSystem.StateManger
{
    public class ReloadState : CharacterState
    {
        public Action OnReload;
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
                    ManagerService.Animation.Play("Armature|reload_ammo_left");
                    ServiceLocator.Instance.Get<IAudioManagementService>().PlaySound(SoundEffect.Reload,false);
                    
                }
                
            }
        }

        public override void UpdateState()
        {
            if (!ManagerService.Animation.IsPlaying("Armature|reload_ammo_left"))
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
            ManagerService.FactoryService.ReleaseState(this);
        }
    }
}