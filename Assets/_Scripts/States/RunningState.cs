using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class RunningState : IState
    {
        protected PlayerController1 playerController;
        public RunningState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void Entry()
        {
            playerController.StartAnimation(Animations.Running);
        }
        public void UpdateLogic()
        {

        }

        public void Exit()
        {
            playerController.StopAnimation(Animations.Running);
        }

    } 
}
