using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class JumpingState : IState
    {
        protected PlayerStateMachine PlayerSm;
        protected PlayerController1 PlayerController;
        public JumpingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public void Entry()
        {
            PlayerController.animator.SetTrigger("Jump");
            PlayerController.PerformJump();
        }
        public void UpdateLogic()
        {
            PlayerController.ApplyGravity();
            if(PlayerController.isGrounded)
            {
                PlayerSm.StateMachine.Fire(Trigger.StoppedJumping);
            }
        }
        public void Exit()
        {
            PlayerController.ResetGravity();
        }
    } 
}
