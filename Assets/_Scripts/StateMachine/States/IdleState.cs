using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        private float idleTime = 0;

        public IdleState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public virtual void Entry()
        {
            Debug.Log("This Happened");
            playerController.animator.SetBool("isIdle", true);
            InputManager.jumpPressed += PlayerJumped;
            InputManager.crouchPressed += PlayerCrouched;
            InputManager.reloadPressed += PlayerReloaded;
        }
        public virtual void UpdateLogic()
        {
            playerController.ApplyGravity();
            CheckGuarding();
            if(InputManager.isAiming)
            {
                PlayerAimed();
            }
            if(InputManager.moveDir != Vector2.zero)
            {
                PlayerMoved();
            }
        }

        private void CheckGuarding()
        {
            idleTime += Time.fixedDeltaTime;
            if (idleTime > 10f)
            {
                idleTime = 0;
                PlayerGuarded();
            }
        }

        public virtual void Exit()
        {
            InputManager.jumpPressed -= PlayerJumped;
            InputManager.crouchPressed -= PlayerCrouched;
            InputManager.reloadPressed -= PlayerReloaded;
            playerController.animator.SetBool("isIdle", false);

        }

        protected void PlayerMoved()
        {
            playerSM.stateMachine.Fire(Trigger.StartedWalking);
        }

        protected void PlayerJumped()
        {
            playerSM.stateMachine.Fire(Trigger.StartedJumping);
        }

        protected void PlayerCrouched()
        {
            playerSM.stateMachine.Fire(Trigger.StartedCrouching);
        }

        protected void PlayerAimed()
        {
            playerSM.stateMachine.Fire(Trigger.StartedAiming);
        }
        protected void PlayerReloaded()
        {
            playerSM.stateMachine.Fire(Trigger.StartedReloading);
        }

        protected void PlayerGuarded()
        {
            playerSM.stateMachine.Fire(Trigger.StartedGuarding);
        }

        protected void PlayerDied()
        {
            playerSM.stateMachine.Fire(Trigger.StartedDying);
        }

    } 
}
