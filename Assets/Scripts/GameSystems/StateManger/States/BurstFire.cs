using System;
using GameSystem.Core;
using GameSystem.Core.ServiceLocator;
using GameSystem.StateManager;
using UnityEngine;

namespace GameSystem.StateManger
{
    public class BurstFire : CharacterState
    {
        private float animationLength;
        private float currentAnimationTime;

        

        public override void Setup(CharacterStateManagerService managerService)
        {
            ManagerService = managerService;
            if (ManagerService.Animation == null)
            {
                throw new ArgumentNullException(nameof(ManagerService.Animation));
            }
            animationLength = ManagerService.Animation["Armature|fire"].length;
        }

        public override void EnterState()
        {
            if (ManagerService.Animation == null)
            {
                Debug.LogError("ManagerService animation is Null");
                return;
            }

            ManagerService.Animation.wrapMode = WrapMode.Loop;
            ManagerService.Animation.Play("Armature|fire");
            currentAnimationTime = 0f;
        }

        public override void UpdateState()
        {
            if (ManagerService.Animation.isPlaying)
            {
                currentAnimationTime += Time.deltaTime;
                if (currentAnimationTime >= animationLength)
                {
                    
                    ServiceLocator.Instance.Get<IAudioManagementService>().PlaySound(SoundEffect.Fire,false);
                    currentAnimationTime = 0f; 
                }
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
