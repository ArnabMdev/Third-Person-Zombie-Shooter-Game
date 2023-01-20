using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleShootingState : IdleState
    {
        public IdleShootingState(PlayerStateMachine playerSm) : base(playerSm)
        {

        }
        public override void Entry()
        {
            PlayerController.animator.SetBool("isIdle", true);
            PlayerController.animator.SetBool("isAiming", true);
            switch (PlayerController.gunMode)
            {
                case GunMode.Single:
                    PlayerController.animator.SetInteger("isShootingBullets", 1);
                    break;
                case GunMode.Burst:
                    PlayerController.animator.SetInteger("isShootingBullets", 2);
                    break;
                case GunMode.Auto:
                    PlayerController.animator.SetInteger("isShootingBullets", 3);
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
            PlayerController.animator.SetInteger("isShootingBullets", 0);
        }

        private void PlayerStoppedShooting()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedShooting);
        }
    } 
}
