using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class RunningState : WalkingState
    {
        public RunningState(PlayerStateMachine playerSm) : base(playerSm)
        {

        }
        public override void Entry()
        {
            base.Entry();
            PlayerController.animator.SetBool("isRunning", true);
        }
        public override void UpdateLogic()
        {
            if(InputManager.IsAiming)
            {
                base.PlayerStartedAiming();
            }
            if(!InputManager.IsRunning)
            {
                PlayerStoppedMoving();
            }
            if(InputManager.MoveDir == Vector2.zero)
            {
                base.PlayerStoppedMoving();
            }
            PlayerController.MovePlayer(InputManager.MoveDir, 2);
        }
        public override void Exit()
        {
            PlayerController.animator.SetBool("isRunning", false);
            base.Exit();
        }

        private void PlayerStoppedRunning()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedRunning);
        }
    } 
}
