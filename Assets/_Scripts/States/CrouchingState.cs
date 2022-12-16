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
        public virtual void Entry()
        {
            playerController.StartAnimation(Animations.Crouching);
        }
        public virtual void UpdateLogic()
        {

        }

        public virtual void Exit()
        {
            playerController.StopAnimation(Animations.Crouching);

        }

    } 
}
