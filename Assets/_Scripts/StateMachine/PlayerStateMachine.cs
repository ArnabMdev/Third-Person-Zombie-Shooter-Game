using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stateless;


namespace com.Arnab.ZombieAppocalypseShooter
{
    public enum Trigger
    {
        StartedWalking,
        StoppedWalking,
        StartedGuarding,
        StoppedGuarding,
        StartedAiming,
        StoppedAiming,
        StartedRunning,
        StoppedRunning,
        StartedJumping,
        StoppedJumping,
        StartedShooting,
        StoppedShooting,
        StartedReloading,
        StoppedReloading,
        StartedCrouching,
        StoppedCrouching,
        StartedDying,
        StoppedDying

    }
    public class PlayerStateMachine : IStateMachine
    {
        public StateMachine<IState, Trigger> stateMachine;
        public PlayerController1 playerController;
        private IState previousState;
        private IdleState idleState, idleGuardingState, idleAimingState, idleShootingState;
        private WalkingState walkingState, strafingState, runningState;
        private CrouchingState crouchingState, crouchWalkingState;
        private JumpingState jumpingState;
        private ReloadingState reloadingState;
        private DyingState dyingState;

        public PlayerStateMachine(PlayerController1 playerController)
        {
            this.playerController = playerController;
        }
        public void InitializeStates()
        {
            idleState = new IdleState(this);
            idleGuardingState = new IdleGuardingState(this);
            idleAimingState = new IdleAimingState(this);
            idleShootingState = new IdleShootingState(this);
            walkingState = new WalkingState(this);
            strafingState = new StrafingState(this);
            runningState = new RunningState(this);
            crouchingState = new CrouchingState(this);
            crouchWalkingState = new CrouchWalkingState(this);
            jumpingState = new JumpingState(this);
            reloadingState = new ReloadingState(this);
            dyingState = new DyingState(this);
        }
        public void InitializeStateMachine()
        {
            stateMachine = new StateMachine<IState, Trigger>(idleState);

            stateMachine
                .Configure(idleState)
                .OnEntry(() => idleState.Entry())
                .OnExit(() => idleState.Exit())
                .Permit(Trigger.StartedGuarding, idleGuardingState)
                .Permit(Trigger.StartedAiming, idleAimingState)
                .Permit(Trigger.StartedWalking, walkingState)
                .Permit(Trigger.StartedRunning, runningState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(idleGuardingState)
                .OnEntry(() => idleGuardingState.Entry())
                .OnExit(() => idleGuardingState.Exit())
                .SubstateOf(idleState)
                .Permit(Trigger.StartedGuarding, idleState)
                .Permit(Trigger.StartedRunning, runningState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedWalking, walkingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(idleAimingState)
                .OnEntry(() => idleAimingState.Entry())
                .OnExit(() => idleAimingState.Exit())
                .SubstateOf(idleState)
                .Permit(Trigger.StoppedAiming, idleState)
                .Permit(Trigger.StartedWalking, strafingState)
                .Permit(Trigger.StartedRunning, runningState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(idleShootingState)
                .OnEntry(() => idleShootingState.Entry())
                .OnExit(() => idleShootingState.Exit())
                .Permit(Trigger.StoppedShooting, idleAimingState);

            stateMachine
                .Configure(walkingState)
                .OnEntry(() => walkingState.Entry())
                .OnExit(() => walkingState.Exit())
                .Permit(Trigger.StoppedWalking, idleState)
                .Permit(Trigger.StartedRunning, runningState)
                .Permit(Trigger.StartedAiming, strafingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(strafingState)
                .OnEntry(() => strafingState.Entry())
                .OnExit(() => strafingState.Exit())
                .SubstateOf(walkingState)
                .Permit(Trigger.StoppedAiming, walkingState)
                .Permit(Trigger.StoppedWalking, idleAimingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(runningState)
                .OnEntry(() => runningState.Entry())
                .OnExit(() => runningState.Exit())
                .Permit(Trigger.StoppedRunning, walkingState)
                .Permit(Trigger.StartedAiming, strafingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => playerController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(crouchingState)
                .OnEntry(() => crouchingState.Entry())
                .OnExit(() => crouchingState.Exit())
                .Permit(Trigger.StoppedCrouching, idleState)
                .Permit(Trigger.StartedJumping, idleState)
                .Permit(Trigger.StartedRunning, runningState)
                .Permit(Trigger.StartedWalking, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(crouchWalkingState)
                .OnEntry(() => crouchWalkingState.Entry())
                .OnExit(() => crouchWalkingState.Exit())
                .SubstateOf(crouchingState)
                .Permit(Trigger.StoppedWalking, crouchingState)
                .Permit(Trigger.StartedJumping, walkingState)
                .Permit(Trigger.StoppedCrouching, walkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(jumpingState)
                .OnEntry(() => jumpingState.Entry())
                .OnExit(() => jumpingState.Exit())
                .PermitDynamic(Trigger.StoppedJumping, () => { return previousState; });

            stateMachine
                .Configure(reloadingState)
                .OnEntry(() => reloadingState.Entry())
                .OnExit(() => reloadingState.Exit())
                .PermitDynamic(Trigger.StoppedReloading, () => { return previousState; });

            stateMachine
                .Configure(dyingState)
                .OnEntry(() => dyingState.Entry())
                .OnExit(() => dyingState.Exit())
                .Permit(Trigger.StoppedDying, idleState);

            stateMachine.OnTransitioned((t) => previousState = t.Source);
            stateMachine.OnUnhandledTrigger((state, trigger) => Debug.Log($"Cant Perform Trigger : {state} from {trigger}"));
        }
    }
}
