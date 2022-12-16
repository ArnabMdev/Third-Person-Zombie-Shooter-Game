using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class JumpingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public JumpingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public void Entry()
        {
            playerController.animator.SetTrigger("Jump");
            playerController.PerformJump();
        }
        public void UpdateLogic()
        {
            playerController.ApplyGravity();
            if(playerController.isGrounded)
            {
                playerSM.stateMachine.Fire(Trigger.StoppedJumping);
            }
        }
        public void Exit()
        {
            playerController.ResetGravity();
        }
    } 
}
