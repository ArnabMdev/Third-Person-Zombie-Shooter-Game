using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Stateless;

namespace com.Arnab.ZombieAppocalypseShooter
{
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
        RunningGuarding,

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
        StoppedWalking
    }

    public class PlayerController1 : MonoBehaviour
    {
        #region StateMachineProperties
        private StateMachine<IState, Trigger> stateMachine;
        private IState IdleState;
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
            
            //IdleState = new IdleState();
            //stateMachine = new StateMachine<IState, Trigger>();
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
            /*if (activeState == playerState.Jumping && characterController.isGrounded)
            {
                ResetGravity();
                activeState = playerState.Walking;
            }
            switch (activeState)
            {
                case playerState.Idle:
                    TurnPlayer(moveDir);
                    ApplyGravity();
                    break;
                case playerState.Walking:
                    TurnPlayer(moveDir);
                    MovePlayer();
                    ApplyGravity();
                    break;
                case playerState.Sprinting:
                    TurnPlayer(moveDir);
                    MovePlayer();
                    ApplyGravity();
                    break;
                case playerState.Jumping:
                    TurnPlayer(moveDir);
                    ApplyGravity();
                    break;
                case playerState.Flying:
                    ControlFlight();
                    break;
                default:
                    break;
            }*/

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
                case Animations.RunningGuarding:
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
                case Animations.RunningGuarding:
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isGuarding", true);
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

        private void MovePlayer(Vector2 moveDir)
        {
            float _walkTrigger = Mathf.Sqrt(Mathf.Pow(moveDir.x, 2) + Mathf.Pow(moveDir.y, 2));
            if (_walkTrigger > turningValueOffset)
            {
                movementVector = new Vector3(moveVector.x * moveSpeed * Time.fixedDeltaTime, 0, moveVector.z * moveSpeed * Time.fixedDeltaTime);
                characterController.Move(movementVector * Time.fixedDeltaTime); 

            }

        }

        private void TurnPlayer(Vector2 inputVector)
        {
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

        #endregion
    }
}

