using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class DyingState : IState
    {
        protected PlayerController1 playerController;
        public DyingState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void Entry()
        {
            playerController.StartAnimation(Animations.Dying);
        }
        public void UpdateLogic()
        {

        }

        public void Exit()
        {
            playerController.StopAnimation(Animations.Dying);

        }

    } 
}
