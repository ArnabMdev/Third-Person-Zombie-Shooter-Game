using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class StrafingState : IState
    {
        private readonly PlayerStateMachine _playerSm;
        private readonly PlayerController1 _playerController;
        private static readonly int IsStrafingInDirection = Animator.StringToHash("isStrafingInDirection");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsAiming = Animator.StringToHash("isAiming");

        public StrafingState(PlayerStateMachine playerSm)
        {
            this._playerSm = playerSm;
            this._playerController = playerSm.PlayerController;
        }
        public void Entry()
        {
            _playerController.animator.SetBool(IsWalking, true);
            _playerController.animator.SetBool(IsAiming, true);
            _playerController.animator.SetInteger(IsStrafingInDirection, 1);
            InputManager.JumpPressed += PlayerJumped;
            InputManager.CrouchPressed += PlayerCrouched;
            InputManager.ReloadPressed += PlayerReloaded;}
        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateLogic()
        {
            if(!InputManager.IsAiming)
            {
                PlayerStoppedAiming();
            }
            if(InputManager.IsRunning)
            {
                PlayerStartedRunning();
            }
            if(InputManager.MoveDir == Vector2.zero)
            {
                PlayerStoppedMoving();
            }
            _playerController.JumpAndGravity(false);
            _playerController.StrafePlayer(InputManager.MoveDir);
        }
        public void Exit()
        {
            _playerController.animator.SetBool(IsAiming, false);
            _playerController.animator.SetInteger(IsStrafingInDirection, 0);
            _playerController.animator.SetBool(IsWalking, false);

        }

        private void PlayerCrouched()
        {
            _playerSm.StateMachine.Fire(Trigger.StartedCrouching);
        }
        private void PlayerStoppedAiming()
        {
            _playerSm.StateMachine.Fire(Trigger.StoppedAiming);
        }
        
        private void PlayerStoppedMoving()
        {
            _playerSm.StateMachine.Fire(Trigger.StoppedWalking);
        }

        private void PlayerReloaded()
        {
            _playerSm.StateMachine.Fire(Trigger.StartedReloading);
        }

        private void PlayerJumped()
        {
            _playerSm.StateMachine.Fire(Trigger.StartedJumping);
        }
        
        protected void PlayerStartedRunning()
        {
            _playerSm.StateMachine.Fire(Trigger.StartedRunning);
        }
    } 
}
