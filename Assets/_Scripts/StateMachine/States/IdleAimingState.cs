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
            InputManager.shootPressed += PlayerShoot;
            playerController.animator.SetBool("isAiming", true);
        }
        public override void UpdateLogic()
        {
            
        }
        public override void Exit()
        {
            playerController.animator.SetBool("isAiming", false);
            base.Exit();
        }

        protected void PlayerShoot()
        {
            playerSM.stateMachine.Fire(Trigger.StartedShooting);
        }
    } 
}
