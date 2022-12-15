using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleState : IState
    {
        protected PlayerController1 playerController;

        public IdleState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void Entry()
        {
            playerController.StartAnimation(Animations.Idle);
        }
        public void UpdateLogic()
        {

        }
        public void Exit()
        {
            playerController.StopAnimation(Animations.Idle);

        }
    } 
}
