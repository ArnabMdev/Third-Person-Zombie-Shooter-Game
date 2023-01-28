using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleAimingState : IdleState
    {
        public IdleAimingState(PlayerStateMachine playerSm) : base(playerSm)
        {

        }
        public override void Entry()
        {
            base.Entry();
            PlayerController.animator.SetBool("isAiming", true);
        }
        public override void UpdateLogic()
        {
            if(!InputManager.IsAiming)
            {
                PlayerStoppedAiming();
            }
            if (InputManager.MoveDir != Vector2.zero)
            {
                PlayerMoved();
            }
            if (InputManager.IsShooting)
            {
                PlayerShoot();
            }
        }
        public override void Exit()
        {
            PlayerController.animator.SetBool("isAiming", false);
            base.Exit();
        }

        protected void PlayerStoppedAiming()
        {
            PlayerSm.StateMachine.Fire(Trigger.StoppedAiming);
        }

        protected void PlayerShoot()
        {
            PlayerSm.StateMachine.Fire(Trigger.StartedShooting);
        }
    } 
}
