using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleGuardingState : IdleState
    {
        public IdleGuardingState(PlayerStateMachine playerSM) : base(playerSM)
        {
            
        }
        public override void Entry()
        {
            base.Entry();
            playerController.animator.SetBool("isGuarding", true);
        }
        public override void UpdateLogic()
        {
            playerController.ApplyGravity();
            if (InputManager.isAiming)
            {
                PlayerAimed();
            }
            if (InputManager.moveDir != Vector2.zero)
            {
                PlayerMoved();
            }
        }
        public override void Exit()
        {
            playerController.animator.SetBool("isGuarding", false);
            base.Exit();
        }
    } 
}
