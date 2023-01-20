using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stateless;


namespace com.Arnab.ZombieAppocalypseShooter
{
    public enum Trigger
    {
        StartedGame,
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
        public StateMachine<IState, Trigger> StateMachine;
        public readonly PlayerController1 PlayerController;
        private IState _previousState;
        private IdleState _initialState, _idleState, _idleGuardingState, _idleAimingState, _idleShootingState;
        private WalkingState _walkingState, _runningState;
        private StrafingState _strafingState;
        private CrouchingState _crouchingState, _crouchWalkingState;
        private JumpingState _jumpingState;
        private ReloadingState _reloadingState;
        private DyingState _dyingState;

        public PlayerStateMachine(PlayerController1 playerController)
        {
            this.PlayerController = playerController;
        }
        public void InitializeStates()
        {
            _initialState = new IdleState(this);
            _idleState = new IdleState(this);
            _idleGuardingState = new IdleGuardingState(this);
            _idleAimingState = new IdleAimingState(this);
            _idleShootingState = new IdleShootingState(this);
            _walkingState = new WalkingState(this);
            _strafingState = new StrafingState(this);
            _runningState = new RunningState(this);
            _crouchingState = new CrouchingState(this);
            _crouchWalkingState = new CrouchWalkingState(this);
            _jumpingState = new JumpingState(this);
            _reloadingState = new ReloadingState(this);
            _dyingState = new DyingState(this);
        }
        public void InitializeStateMachine()
        {
            StateMachine = new StateMachine<IState, Trigger>(_initialState);

            StateMachine
                .Configure(_initialState)
                .Permit(Trigger.StartedGame, _idleState);

            StateMachine
                .Configure(_idleState)
                .OnEntry(() => _idleState.Entry())
                .OnExit(() => _idleState.Exit())
                .Permit(Trigger.StartedGuarding, _idleGuardingState)
                .Permit(Trigger.StartedAiming, _idleAimingState)
                .Permit(Trigger.StartedWalking, _walkingState)
                .Permit(Trigger.StartedRunning, _runningState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_idleGuardingState)
                .OnEntry(() => _idleGuardingState.Entry())
                .OnExit(() => _idleGuardingState.Exit())
                // .SubstateOf(_idleState)
                .Permit(Trigger.StartedGuarding, _idleState)
                .Permit(Trigger.StartedRunning, _runningState)
                .Permit(Trigger.StartedAiming, _idleAimingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedWalking, _walkingState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_idleAimingState)
                .OnEntry(() => _idleAimingState.Entry())
                .OnExit(() => _idleAimingState.Exit())
                // .SubstateOf(_idleState)
                .Permit(Trigger.StoppedAiming, _idleState)
                .Permit(Trigger.StartedShooting, _idleShootingState)
                .Permit(Trigger.StartedWalking, _strafingState)
                .Permit(Trigger.StartedRunning, _runningState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_idleShootingState)
                .OnEntry(() => _idleShootingState.Entry())
                .OnExit(() => _idleShootingState.Exit())
                // .SubstateOf(_idleState)
                .Permit(Trigger.StoppedShooting, _idleAimingState);

            StateMachine
                .Configure(_walkingState)
                .OnEntry(() => _walkingState.Entry())
                .OnExit(() => _walkingState.Exit())
                .Permit(Trigger.StoppedWalking, _idleState)
                .Permit(Trigger.StartedRunning, _runningState)
                .Permit(Trigger.StartedAiming, _strafingState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchWalkingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_strafingState)
                .OnEntry(() => _strafingState.Entry())
                .OnExit(() => _strafingState.Exit())
                .Permit(Trigger.StoppedAiming, _walkingState)
                .Permit(Trigger.StoppedWalking, _idleAimingState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchWalkingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_runningState)
                .OnEntry(() => _runningState.Entry())
                .OnExit(() => _runningState.Exit())
                .Permit(Trigger.StoppedRunning, _walkingState)
                .Permit(Trigger.StoppedWalking, _idleState)
                .Permit(Trigger.StartedAiming, _strafingState)
                .PermitIf(Trigger.StartedJumping, _jumpingState, () => PlayerController.isGrounded)
                .Permit(Trigger.StartedCrouching, _crouchWalkingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_crouchingState)
                .OnEntry(() => _crouchingState.Entry())
                .OnExit(() => _crouchingState.Exit())
                .Permit(Trigger.StoppedCrouching, _idleState)
                .Permit(Trigger.StartedJumping, _idleState)
                .Permit(Trigger.StartedRunning, _runningState)
                .Permit(Trigger.StartedWalking, _crouchWalkingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_crouchWalkingState)
                .OnEntry(() => _crouchWalkingState.Entry())
                .OnExit(() => _crouchWalkingState.Exit())
                .SubstateOf(_crouchingState)
                .Permit(Trigger.StoppedWalking, _crouchingState)
                .Permit(Trigger.StartedJumping, _walkingState)
                .Permit(Trigger.StoppedCrouching, _walkingState)
                .Permit(Trigger.StartedReloading, _reloadingState)
                .Permit(Trigger.StartedDying, _dyingState);

            StateMachine
                .Configure(_jumpingState)
                .OnEntry(() => _jumpingState.Entry())
                .OnExit(() => _jumpingState.Exit())
                .PermitDynamic(Trigger.StoppedJumping, () => _previousState);

            StateMachine
                .Configure(_reloadingState)
                .OnEntry(() => _reloadingState.Entry())
                .OnExit(() => _reloadingState.Exit())
                .PermitDynamic(Trigger.StoppedReloading, () => _previousState);

            StateMachine
                .Configure(_dyingState)
                .OnEntry(() => _dyingState.Entry())
                .OnExit(() => _dyingState.Exit())
                .Permit(Trigger.StoppedDying, _idleState);

            StateMachine.OnTransitioned((t) => {
                _previousState = t.Source;
                //Debug.Log(t.Source + "->" + t.Destination);
                });
            StateMachine.OnUnhandledTrigger((state, trigger) => Debug.Log($"Cant Perform Trigger : {trigger} from {state}"));
            
        }
    }
}
