using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Stateless;

namespace CharecterControllerBased
{
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
        private StateMachine<IState, Trigger> stateMachine;    

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
        private float turnSmoothVelocity;
        private Vector2 moveDir;
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
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            activeState = playerState.Idle;
            moveVector = Vector3.zero;
            movementVector = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (activeState == playerState.Jumping && characterController.isGrounded)
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
            }

        }
        #endregion

        #region InputCallBacks
        public void Move(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>();
            if (activeState == playerState.Jumping || activeState == playerState.Flying)
            {
                return;
            }
            if (context.performed)
            {
                activeState = playerState.Walking;
            }
            if (context.canceled)
            {
                activeState = playerState.Idle;
            }


        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (activeState == playerState.Jumping || activeState == playerState.Flying)
            {
                return;
            }
            activeState = playerState.Jumping;
            PerformJump();


        }
        public void Sprint(InputAction.CallbackContext context)
        {
            if (activeState == playerState.Flying || activeState == playerState.Jumping)
            {
                return;
            }
            if (context.started)
            {
                activeState = playerState.Sprinting;
            }
            if (context.canceled)
            {
                activeState = playerState.Walking;
            }
        }

        public void ToggleFlying(InputAction.CallbackContext context)
        {
            if (!canFly)
                return;
            if (context.performed)
            {
                if (activeState == playerState.Flying)
                {
                    activeState = playerState.Walking;
                    transform.Rotate(Vector3.right, -90);
                }
                else
                {
                    activeState = playerState.Flying;
                    transform.Rotate(Vector3.right, 90);
                }
            }
        }


        public void ControlPitch(InputAction.CallbackContext context)
        {
            if (activeState != playerState.Flying)
                return;
            Debug.Log("pitch");
            activePitch = context.ReadValue<float>();
            activePitch *= pitchPower;
        }

        public void ControlRoll(InputAction.CallbackContext context)
        {
            if (activeState != playerState.Flying)
                return;
            activeRoll = context.ReadValue<float>();
            activeRoll *= rollPower;
        }

        public void ControlYaw(InputAction.CallbackContext context)
        {
            if (activeState != playerState.Flying)
                return;
            activeYaw = context.ReadValue<float>();
            activeYaw *= yawPower;
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (activeState == playerState.Flying || activeState == playerState.Jumping)
            {
                return;
            }
            if (context.started)
            {
                activeState = playerState.Crouching;
            }
            if (context.canceled)
            {
                activeState = playerState.Crouching;
            }
        }

        #endregion

        #region ControllerMethods
        private void PerformJump()
        {
            movementVector.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        private void MovePlayer()
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

