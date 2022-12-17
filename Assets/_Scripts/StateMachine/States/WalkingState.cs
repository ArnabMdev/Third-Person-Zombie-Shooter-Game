using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class WalkingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public WalkingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public virtual void Entry()
        {
            playerController.animator.SetBool("isWalking", true);
            InputManager.jumpPressed += PlayerJumped;
            InputManager.crouchPressed += PlayerCrouched;
            InputManager.reloadPressed += PlayerReloaded;
        }
        public virtual void UpdateLogic()
        {
            if (InputManager.isAiming)
            {
                PlayerStartedAiming();
            }
            if(InputManager.isRunning)
            {
                PlayerStartedRunning();
            }
            if(InputManager.moveDir == Vector2.zero)
            {
                PlayerStoppedMoving();
            }
            playerController.ApplyGravity();
            playerController.MovePlayer(InputManager.moveDir, 1);
        }
        public virtual void Exit()
        {
            playerController.animator.SetBool("isWalking", false);
            InputManager.jumpPressed -= PlayerJumped;
            InputManager.crouchPressed -= PlayerCrouched;
            InputManager.reloadPressed -= PlayerReloaded;
        }

        protected void PlayerStoppedMoving()
        {
            playerSM.stateMachine.Fire(Trigger.StoppedWalking);
        }

        protected void PlayerCrouched()
        {
            playerSM.stateMachine.Fire(Trigger.StartedCrouching);
        }

        protected void PlayerJumped()
        {
            playerSM.stateMachine.Fire(Trigger.StartedJumping);
        }

        protected void PlayerStartedRunning()
        {
            playerSM.stateMachine.Fire(Trigger.StartedRunning);
        }

        protected void PlayerStartedAiming()
        {
            playerSM.stateMachine.Fire(Trigger.StartedAiming);
        }

        protected void PlayerReloaded()
        {
            playerSM.stateMachine.Fire(Trigger.StartedReloading);
        }
    } 
}
