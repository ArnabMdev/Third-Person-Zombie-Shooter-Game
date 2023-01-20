using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleState : IState
    {
        protected readonly PlayerStateMachine PlayerSm;
        protected readonly PlayerController1 PlayerController;
        private float _idleTime = 0;
        private static readonly int IsIdle = Animator.StringToHash("isIdle");

        public IdleState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public virtual void Entry()
        {
            // Debug.Log("This Happened");
            PlayerController.animator.SetBool(IsIdle, true);
            InputManager.jumpPressed += PlayerJumped;
            InputManager.crouchPressed += PlayerCrouched;
            InputManager.reloadPressed += PlayerReloaded;
        }
        public virtual void UpdateLogic()
        {
            PlayerController.ApplyGravity();
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
            _idleTime += Time.fixedDeltaTime;
            if (_idleTime > 10f)
            {
                _idleTime = 0;
                PlayerGuarded();
            }
        }

        public virtual void Exit()
        {
            InputManager.jumpPressed -= PlayerJumped;
            InputManager.crouchPressed -= PlayerCrouched;
            InputManager.reloadPressed -= PlayerReloaded;
            PlayerController.animator.SetBool("isIdle", false);

        }

        protected void PlayerMoved()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedWalking);
        }

        protected void PlayerJumped()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedJumping);
        }

        protected void PlayerCrouched()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedCrouching);
        }

        protected void PlayerAimed()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedAiming);
        }
        protected void PlayerReloaded()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedReloading);
        }

        protected void PlayerGuarded()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedGuarding);
        }

        protected void PlayerDied()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedDying);
        }

    } 
}
