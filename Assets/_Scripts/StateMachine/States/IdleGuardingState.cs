using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class IdleGuardingState : IdleState
    {
        public IdleGuardingState(PlayerStateMachine playerSm) : base(playerSm)
        {
            
        }
        public override void Entry()
        {
            base.Entry();
            PlayerController.animator.SetBool("isGuarding", true);
        }
        public override void UpdateLogic()
        {
            PlayerController.ApplyGravity();
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
            PlayerController.animator.SetBool("isGuarding", false);
            base.Exit();
        }
    } 
}
