using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class ReloadingState : IState
    {
        protected PlayerController1 playerController;
        public ReloadingState(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void Entry()
        {

        }
        public void UpdateLogic()
        {

        }
        public void Exit()
        {

        }
    } 
}
