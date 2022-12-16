using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Stateless;
using Stateless.Graph;

namespace com.Arnab.ZombieAppocalypseShooter
{

    #region Enums
    public enum Animations
    {
        Idle,
        IdleGuarding,
        IdleAiming,
        IdleShootingSingle,
        IdleShootingBurst,
        IdleShootingAuto,
        Reloading,
        Dying,
        Jumping,
        StrafingFront,
        StrafingLeft,
        StrafingRight,
        StrafingBack,
        Running,
        Walking,
        Crouching

    }
    public enum playerState
    {
        Idle = 0,
        Walking = 1,
        Sprinting = 2,
        Crouching = 3,
        Jumping = 4,
        Flying = 5,

    }
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
    #endregion

    public class PlayerController1 : MonoBehaviour
    {
        #region StateMachineProperties
        private StateMachine<IState, Trigger> stateMachine;
        private IState previousState;
        private IdleState idleState, idleGuardingState, idleAimingState, idleShootingState;
        private WalkingState walkingState, strafingState;
        private CrouchingState crouchingState,crouchWalkingState;
        private RunningState runningState;
        private JumpingState jumpingState;
        private ReloadingState reloadingState;
        private DyingState dyingState;
        

        
        #endregion

        #region Properties
        [Header("Movement Properties")]
        public playerState activeState;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float lookSpeed;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private bool isFlying;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] float turningValueOffset;
        private Animator animator;
        private float turnSmoothVelocity;
        private Vector3 movementVector;


        [Header("Gun Properties")]
        [SerializeField] private GameObject gun;
        [SerializeField] private Texture2D xHairImage;
        [SerializeField] private Vector2 xHairOffset;


        [Header("Flight Properties")]
        [SerializeField] private bool canFly;
        [SerializeField] private float propulsionSpeed;
        [SerializeField] private float pitchPower, yawPower, rollPower;
        private float activePitch, activeYaw, activeRoll;
        private Vector3 moveVector;
        private CharacterController characterController;

        #endregion

        #region UnityMethods


        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        private void Awake()
        {
            idleState = new IdleState(this);
            idleGuardingState = new IdleGuardingState(this);
            idleShootingState = new IdleShootingState(this);
            idleAimingState = new IdleAimingState(this);
            walkingState = new WalkingState(this);
            strafingState = new StrafingState(this);
            runningState = new RunningState(this);
            crouchingState = new CrouchingState(this);
            crouchWalkingState = new CrouchWalkingState(this);
            reloadingState = new ReloadingState(this);
            dyingState = new DyingState(this);
            InitializeStateMachine();
            
        }
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            activeState = playerState.Idle;
            moveVector = Vector3.zero;
            movementVector = Vector3.zero;
        }

        private void FixedUpdate()
        {
            stateMachine.State.UpdateLogic();

        }
        #endregion

        #region ControllerMethods

        public void StopAnimation(Animations animation)
        {
            switch (animation)
            {
                case Animations.Idle:
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isGuarding", false);
                    break;
                case Animations.IdleGuarding:
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isGuarding", false);
                    break;
                case Animations.IdleAiming:
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isAiming", false);
                    break;
                case Animations.IdleShootingSingle:
                    animator.SetInteger("isShootingBullets", 0);
                    break;
                case Animations.IdleShootingBurst:
                    animator.SetInteger("isShootingBullets", 0);
                    break;
                case Animations.IdleShootingAuto:
                    animator.SetInteger("isShootingBullets", 0);
                    break;
                case Animations.Reloading:
                    break;
                case Animations.Dying:
                    break;
                case Animations.Jumping:
                    break;
                case Animations.StrafingFront:
                    animator.SetInteger("isStrafingInDirection", 0);
                    break;
                case Animations.StrafingLeft:
                    animator.SetInteger("isStrafingInDirection", 0);
                    break;
                case Animations.StrafingRight:
                    animator.SetInteger("isStrafingInDirection", 0);
                    break;
                case Animations.StrafingBack:
                    animator.SetInteger("isStrafingInDirection", 0);
                    break;
                case Animations.Running:
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isGuarding", false);
                    break;
                case Animations.Walking:
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isGuarding", false);
                    break;
                default:
                    break;
            }
        }

        public void StartAnimation(Animations animation)
        {
            switch (animation)
            {
                case Animations.Idle:
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isGuarding", false);
                    break;
                case Animations.IdleGuarding:
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isGuarding", true);
                    break;
                case Animations.IdleAiming:
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isAiming", true);
                    break;
                case Animations.IdleShootingSingle:
                    animator.SetInteger("isShootingBullets", 1);
                    break;
                case Animations.IdleShootingBurst:
                    animator.SetInteger("isShootingBullets", 4);
                    break;
                case Animations.IdleShootingAuto:
                    animator.SetInteger("isShootingBullets", 8);
                    break;
                case Animations.Reloading:
                    animator.SetTrigger("Reload");
                    break;
                case Animations.Dying:
                    animator.SetTrigger("Die");
                    break;
                case Animations.Jumping:
                    animator.SetTrigger("Jump");
                    break;
                case Animations.StrafingFront:
                    animator.SetInteger("isStrafingInDirection", 1);
                    break;
                case Animations.StrafingLeft:
                    animator.SetInteger("isStrafingInDirection", 2);
                    break;
                case Animations.StrafingRight:
                    animator.SetInteger("isStrafingInDirection", 3);
                    break;
                case Animations.StrafingBack:
                    animator.SetInteger("isStrafingInDirection", 4);
                    break;
                case Animations.Running:
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isGuarding", false);
                    break;
                case Animations.Walking:
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isGuarding", true);
                    break;
                case Animations.Crouching:
                    animator.SetBool("isCrouching", true);
                    break;
                default:
                    break;
            }
        }
        private void PerformJump()
        {
            movementVector.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        public void MovePlayer()
        {
            var moveDir = InputManager.moveDir;
            float _walkTrigger = Mathf.Sqrt(Mathf.Pow(moveDir.x, 2) + Mathf.Pow(moveDir.y, 2));
            if (_walkTrigger > turningValueOffset)
            {
                movementVector = new Vector3(moveVector.x * moveSpeed * Time.fixedDeltaTime, 0, moveVector.z * moveSpeed * Time.fixedDeltaTime);
                characterController.Move(movementVector * Time.fixedDeltaTime); 

            }

        }

        public void TurnPlayer()
        {
            var inputVector = InputManager.moveDir;
            Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y); ;
            if (inputVector.magnitude >= 0.1f)
            {
                float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveVector = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }
        }

        private void ControlFlight()
        {
            characterController.Move(transform.up * propulsionSpeed);
            transform.Rotate(activePitch * pitchPower * Time.fixedDeltaTime, -activeRoll * Time.fixedDeltaTime, -activeYaw * yawPower * Time.fixedDeltaTime, Space.Self);
        }

        private void ApplyGravity()
        {
            if(characterController.isGrounded)
            {
                return;
            } 
            movementVector.y += gravityValue * Time.fixedDeltaTime;
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        private void ResetGravity()
        {
            if(movementVector.y < 0)
            {
                movementVector.y = 0;
            }
        }

        private void InitializeStateMachine()
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
                .PermitIf(Trigger.StartedJumping, jumpingState, () => characterController.isGrounded)
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
                .PermitIf(Trigger.StartedJumping, jumpingState, () => characterController.isGrounded)
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
                .PermitIf(Trigger.StartedJumping, jumpingState,() => characterController.isGrounded)
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
                .PermitIf(Trigger.StartedJumping, jumpingState, () => characterController.isGrounded)
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
                .PermitIf(Trigger.StartedJumping, jumpingState, () => characterController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(runningState)
                .OnEntry(() => runningState.Entry())
                .OnExit(() => runningState.Exit())
                .Permit(Trigger.StoppedRunning, walkingState)
                .Permit(Trigger.StartedAiming, strafingState)
                .PermitIf(Trigger.StartedJumping, jumpingState, () => characterController.isGrounded)
                .Permit(Trigger.StartedCrouching, crouchWalkingState)
                .Permit(Trigger.StartedReloading, reloadingState)
                .Permit(Trigger.StartedDying, dyingState);

            stateMachine
                .Configure(crouchingState)
                .OnEntry(()=>crouchingState.Entry())
                .OnExit(()=>crouchingState.Exit())
                .Permit(Trigger.StoppedCrouching,idleState)
                .Permit(Trigger.StartedJumping,idleState)
                .Permit(Trigger.StartedRunning,runningState)
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
            stateMachine.OnUnhandledTrigger((state,trigger) => Debug.Log($"Cant Perform Trigger : {state} from {trigger}"));
        }

        #endregion
    }
}

