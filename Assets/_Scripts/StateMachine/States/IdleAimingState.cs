using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleAimingState : IdleState
    {
        public IdleAimingState(PlayerStateMachine playerSM) : base(playerSM)
        {

        }
        public override void Entry()
        {
            base.Entry();
            playerController.animator.SetBool("isAiming", true);
        }
        public override void UpdateLogic()
        {
            if(!InputManager.isAiming)
            {
                PlayerStoppedAiming();
            }
            if (InputManager.moveDir != Vector2.zero)
            {
                PlayerMoved();
            }
            if (InputManager.isShooting)
            {
                PlayerShoot();
            }
        }
        public override void Exit()
        {
            playerController.animator.SetBool("isAiming", false);
            base.Exit();
        }

        protected void PlayerStoppedAiming()
        {
            playerSM.stateMachine.Fire(Trigger.StoppedAiming);
        }

        protected void PlayerShoot()
        {
            playerSM.stateMachine.Fire(Trigger.StartedShooting);
        }
    } 
}
