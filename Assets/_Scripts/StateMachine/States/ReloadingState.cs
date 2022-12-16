using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class ReloadingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public ReloadingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public void Entry()
        {
            playerController.animator.SetTrigger("Reload");
        }
        public void UpdateLogic()
        {

        }
        public void Exit()
        {

        }
    } 
}
