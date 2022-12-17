using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class RunningState : WalkingState
    {
        public RunningState(PlayerStateMachine playerSM) : base(playerSM)
        {

        }
        public override void Entry()
        {
            base.Entry();
            playerController.animator.SetBool("isRunning", true);
        }
        public override void UpdateLogic()
        {
            if(InputManager.isAiming)
            {
                base.PlayerStartedAiming();
            }
            if(!InputManager.isRunning)
            {
                PlayerStoppedMoving();
            }
            if(InputManager.moveDir == Vector2.zero)
            {
                base.PlayerStoppedMoving();
            }
            playerController.ApplyGravity();
            playerController.MovePlayer(InputManager.moveDir, 2);
        }
        public override void Exit()
        {
            playerController.animator.SetBool("isRunning", false);
            base.Exit();
        }

        private void PlayerStoppedRunning()
        {
            playerSM.stateMachine.Fire(Trigger.StoppedRunning);
        }
    } 
}
