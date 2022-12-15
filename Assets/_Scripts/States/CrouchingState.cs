using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class CrouchingState : IState
    {
        protected PlayerController1 playerController;
        public CrouchingState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void Entry()
        {
            playerController.StartAnimation(Animations.Crouching);
        }
        public void UpdateLogic()
        {

        }

        public void Exit()
        {
            playerController.StopAnimation(Animations.Crouching);

        }

    } 
}
