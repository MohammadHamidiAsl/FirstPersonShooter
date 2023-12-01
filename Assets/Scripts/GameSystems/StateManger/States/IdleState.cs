using System;
using GameSystem.StateManager;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSystem.StateManger
{
    public class IdleState : CharacterState
    {
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
                    ManagerService.Animation.CrossFade("Armature|idle",.3f);
                    
                }
                
            }
           
        }


        public override  void UpdateState()
        {
            
        }

        public override  void ExitState()
        {
           Reset();
        }

        public override  void Reset()
        {
            ManagerService.FactoryService.ReleaseState(this);
        }
    }
}