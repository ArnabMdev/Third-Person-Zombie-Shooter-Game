using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class StrafingState : WalkingState
    {
        public StrafingState(PlayerController1 playerController1) : base(playerController1)
        {
            
        }
        public override void Entry()
        {
            playerController.StartAnimation(Animations.StrafingFront);
        }
        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
        public override void Exit()
        {
            base.Exit();
        }
    } 
}
