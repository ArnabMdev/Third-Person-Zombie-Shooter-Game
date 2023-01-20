using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class CrouchingState : IState
    {
        protected PlayerStateMachine PlayerSm;
        protected PlayerController1 PlayerController;
        public CrouchingState(PlayerStateMachine playerSm)
        {
            this.PlayerSm = playerSm;
            this.PlayerController = playerSm.PlayerController;
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
