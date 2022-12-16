using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class StrafingState : WalkingState
    {
        public StrafingState(PlayerStateMachine playerSM) : base(playerSM)
        {
            
        }
        public override void Entry()
        {
            base.Entry();
            playerController.animator.SetInteger("isStrafingInDirection", 1);
        }
        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
        public override void Exit()
        {
            playerController.animator.SetInteger("isStrafingInDirection", 0);
            base.Exit();
        }
    } 
}
