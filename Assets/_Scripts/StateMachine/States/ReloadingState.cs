using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class ReloadingState : IState
    {
        protected PlayerStateMachine PlayerSm;
        protected PlayerController1 PlayerController;
        public ReloadingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public void Entry()
        {
            PlayerController.animator.SetTrigger("Reload");
        }
        public void UpdateLogic()
        {

        }
        public void Exit()
        {

        }
    } 
}
