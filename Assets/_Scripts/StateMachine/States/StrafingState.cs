using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class StrafingState : WalkingState
    {
        public StrafingState(PlayerStateMachine playerSM) : base(playerSM)
        {
            
        }
        public override void Entry()
        {
            base.Entry();
            playerController.animator.SetBool("isAiming", true);
            playerController.animator.SetInteger("isStrafingInDirection", 1);
        }
        public override void UpdateLogic()
        {
            if(!InputManager.isAiming)
            {
                PlayerStoppedAiming();
            }
            if(InputManager.moveDir == Vector2.zero)
            {
                base.PlayerStoppedMoving();
            }
            playerController.ApplyGravity();
            playerController.StrafePlayer(InputManager.moveDir);
        }
        public override void Exit()
        {
            playerController.animator.SetBool("isAiming", false);
            playerController.animator.SetInteger("isStrafingInDirection", 0);
            base.Exit();
        }

        private void PlayerStoppedAiming()
        {
            playerSM.stateMachine.Fire(Trigger.StoppedAiming);
        }
    } 
}
