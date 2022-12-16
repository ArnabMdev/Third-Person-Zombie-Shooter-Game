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
        private float idleTime;

        public IdleState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public virtual void Entry()
        {
            playerController.animator.SetBool("isIdle", true);
            idleTime = Time.time;   
            InputManager.movePressed += PlayerMoved;
            InputManager.jumpPressed += PlayerJumped;
            InputManager.aimPressed += PlayerAimed;
            InputManager.crouchPressed += PlayerCrouched;
            InputManager.reloadPressed += PlayerReloaded;
        }
        public virtual void UpdateLogic()
        {
            if(Mathf.Abs(Time.time - idleTime) > 10f)
            {
                PlayerGuarded();
            }
        }
        public virtual void Exit()
        {
            InputManager.movePressed -= PlayerMoved;
            InputManager.jumpPressed -= PlayerJumped;
            InputManager.aimPressed -= PlayerAimed;
            InputManager.crouchPressed -= PlayerCrouched;
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
