using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class DyingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public DyingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public void Entry()
        {
            playerController.animator.SetTrigger("Die");
        }
        public void UpdateLogic()
        {

        }

        public void Exit()
        {
            

        }

    } 
}
