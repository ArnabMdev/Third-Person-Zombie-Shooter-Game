using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class WalkingState : IState
    {
        protected PlayerController1 playerController;
        public WalkingState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public virtual void Entry()
        {
            playerController.StartAnimation(Animations.Walking);
        }
        public virtual void UpdateLogic()
        {
            playerController.TurnPlayer();
            playerController.MovePlayer();
        }
        public virtual void Exit()
        {
            playerController.StopAnimation(Animations.Walking);
        }
    } 
}
