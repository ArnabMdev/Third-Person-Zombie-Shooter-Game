using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class StrafingState : IState
    {
        private readonly PlayerStateMachine PlayerSm;
        private readonly PlayerController1 PlayerController;
        private static readonly int IsStrafingInDirection = Animator.StringToHash("isStrafingInDirection");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsAiming = Animator.StringToHash("isAiming");

        public StrafingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public void Entry()
        {
            PlayerController.animator.SetBool(IsWalking, true);
            PlayerController.animator.SetBool(IsAiming, true);
            PlayerController.animator.SetInteger(IsStrafingInDirection, 1);
            InputManager.jumpPressed += PlayerJumped;
            InputManager.crouchPressed += PlayerCrouched;
            InputManager.reloadPressed += PlayerReloaded;}
        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateLogic()
        {
            if(!InputManager.isAiming)
            {
                PlayerStoppedAiming();
            }
            if(InputManager.isRunning)
            {
                PlayerStartedRunning();
            }
            if(InputManager.moveDir == Vector2.zero)
            {
                PlayerStoppedMoving();
            }
            PlayerController.ApplyGravity();
            PlayerController.StrafePlayer(InputManager.moveDir);
        }
        public void Exit()
        {
            PlayerController.animator.SetBool(IsAiming, false);
            PlayerController.animator.SetInteger(IsStrafingInDirection, 0);
            PlayerController.animator.SetBool(IsWalking, false);

        }

        private void PlayerCrouched()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedCrouching);
        }
        private void PlayerStoppedAiming()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedAiming);
        }
        
        private void PlayerStoppedMoving()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedWalking);
        }

        private void PlayerReloaded()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedReloading);
        }

        private void PlayerJumped()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedJumping);
        }
        
        protected void PlayerStartedRunning()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedRunning);
        }
    } 
}
