using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class WalkingState : IState
    {
        protected PlayerStateMachine playerSM;
        protected PlayerController1 playerController;
        public WalkingState(PlayerStateMachine playerSM)
        {
            this.playerSM = playerSM;
            this.playerController = playerSM.playerController;
        }
        public virtual void Entry()
        {
            playerController.animator.SetBool("isWalking", true);
        }
        public virtual void UpdateLogic()
        {
            playerController.TurnPlayer();
            playerController.MovePlayer();
        }
        public virtual void Exit()
        {
            playerController.animator.SetBool("isWalking", false);

        }
    } 
}
