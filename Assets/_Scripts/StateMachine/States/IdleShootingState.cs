using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleShootingState : IdleState
    {
        public IdleShootingState(PlayerStateMachine playerSM) : base(playerSM)
        {

        }
        public override void Entry()
        {
            playerController.animator.SetBool("isIdle", true);
            playerController.animator.SetBool("isAiming", true);
            switch (playerController.gunMode)
            {
                case GunMode.Single:
                    playerController.animator.SetInteger("isShootingBullets", 1);
                    break;
                case GunMode.Burst:
                    playerController.animator.SetInteger("isShootingBullets", 2);
                    break;
                case GunMode.Auto:
                    playerController.animator.SetInteger("isShootingBullets", 3);
                    break;
                default:
                    break;
            }

        }
        public override void UpdateLogic()
        {
            if(!InputManager.isShooting)
            {
                PlayerStoppedShooting();
            }
        }
        public override void Exit()
        {
            playerController.animator.SetInteger("isShootingBullets", 0);
        }

        private void PlayerStoppedShooting()
        {
            playerSM.stateMachine.Fire(Trigger.StoppedShooting);
        }
    } 
}
