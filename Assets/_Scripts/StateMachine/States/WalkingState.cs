using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class WalkingState : IState
    {
        protected PlayerStateMachine PlayerSm;
        protected PlayerController1 PlayerController;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        public WalkingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public virtual void Entry()
        {
            // Debug.Log("Walk start");
            PlayerController.animator.SetBool(IsWalking, true);
            InputManager.JumpPressed += PlayerJumped;
            InputManager.CrouchPressed += PlayerCrouched;
            InputManager.ReloadPressed += PlayerReloaded;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void UpdateLogic()
        {
            if (InputManager.IsAiming)
            {
                PlayerStartedAiming();
            }
            if(InputManager.IsRunning)
            {
                PlayerStartedRunning();
            }
            if(InputManager.MoveDir == Vector2.zero)
            {
                PlayerStoppedMoving();
            }
            // PlayerController.ApplyGravity();
            PlayerController.MovePlayer(InputManager.MoveDir, 1);
            PlayerController.JumpAndGravity(false);
        }
        public virtual void Exit()
        {
            PlayerController.animator.SetBool(IsWalking, false);
            InputManager.JumpPressed -= PlayerJumped;
            InputManager.CrouchPressed -= PlayerCrouched;
            InputManager.ReloadPressed -= PlayerReloaded;
        }

        protected void PlayerStoppedMoving()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedWalking);
        }

        protected void PlayerCrouched()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedCrouching);
        }

        protected void PlayerJumped()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedJumping);
        }

        protected void PlayerStartedRunning()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedRunning);
        }

        protected void PlayerStartedAiming()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedAiming);
        }

        protected void PlayerReloaded()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedReloading);
        }
    } 
}
