using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class DyingState : IState
    {
        protected PlayerStateMachine PlayerSm;
        protected PlayerController1 PlayerController;
        public DyingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
        }
        public void Entry()
        {
            PlayerController.animator.SetTrigger("Die");
        }
        public void UpdateLogic()
        {

        }

        public void Exit()
        {
            

        }

    } 
}
