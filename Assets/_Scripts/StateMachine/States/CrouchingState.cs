using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class CrouchingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public CrouchingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public virtual void Entry()
        {
            
        }
        public virtual void UpdateLogic()
        {

        }

        public virtual void Exit()
        {

        }

    } 
}
