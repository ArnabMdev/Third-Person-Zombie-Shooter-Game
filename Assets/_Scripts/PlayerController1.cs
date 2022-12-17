using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Stateless;
using Stateless.Graph;


namespace com.Arnab.ZombieAppocalypseShooter
{

    #region Enums
    public enum GunMode
    {
        Single = 0,
        Burst = 1,
        Auto = 2

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

    #endregion

    public class PlayerController1 : MonoBehaviour
    {
        #region Properties
        [Header("Movement Properties")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float strafeSpeed;
        [SerializeField] private float lookSpeed;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private bool isFlying;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] float turningValueOffset;
        public bool isGrounded => characterController.isGrounded;
        [HideInInspector] public Animator animator;
        private float turnSmoothVelocity;
        private Vector3 movementVector;


        [Header("Gun Properties")]
        [SerializeField] private GameObject gun;
        [SerializeField] private Texture2D xHairImage;
        [SerializeField] private Vector2 xHairOffset;
        public GunMode gunMode = GunMode.Auto;


        [Header("Flight Properties")]
        [SerializeField] private bool canFly;
        [SerializeField] private float propulsionSpeed;
        [SerializeField] private float pitchPower, yawPower, rollPower;
        private float activePitch, activeYaw, activeRoll;
        private Vector3 moveVector;

        private PlayerStateMachine playerSM;
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
            playerSM = new PlayerStateMachine(this);
            playerSM.InitializeStates();
            playerSM.InitializeStateMachine();
        }
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            moveVector = Vector3.zero;
            movementVector = Vector3.zero;
            playerSM.stateMachine.Fire(Trigger.StartedGame);
            //playerSM.stateMachine.State.Entry();

        }

        private void FixedUpdate()
        {
            playerSM.stateMachine.State.UpdateLogic();

        }
        #endregion

        #region ControllerMethods

        public void PerformJump()
        {
            movementVector.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        public void StrafePlayer(Vector2 moveDir)
        {
            var strafeVector = Vector3.zero;
            if(moveDir.x == 1)
            {
                animator.SetInteger("isStrafingInDirection", 2);
                strafeVector = transform.right * strafeSpeed;
            }
            else if(moveDir.x == -1)
            {
                animator.SetInteger("isStrafingInDirection", 3);
                strafeVector = -1 * transform.right * strafeSpeed;
            }
            else if(moveDir.y == 1)
            {
                animator.SetInteger("isStrafingInDirection", 1);
                strafeVector = transform.forward * strafeSpeed;
            }
            else if(moveDir.y == -1)
            {
                animator.SetInteger("isStrafingInDirection", 4);
                strafeVector = -1 * transform.forward * strafeSpeed;
            }
            Debug.Log(strafeVector);
            characterController.Move(strafeVector);
        }

        public void MovePlayer(Vector2 moveDir, int speedMultiplier)
        {
            TurnPlayer(moveDir);
            float _walkTrigger = Mathf.Sqrt(Mathf.Pow(moveDir.x, 2) + Mathf.Pow(moveDir.y, 2));
            if (_walkTrigger > turningValueOffset)
            {
                movementVector = new Vector3(moveVector.x * moveSpeed * Time.fixedDeltaTime, 0, moveVector.z * moveSpeed * Time.fixedDeltaTime);
                characterController.Move(movementVector * speedMultiplier * Time.fixedDeltaTime); 

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

        public void ApplyGravity()
        {
            if(characterController.isGrounded)
            {
                return;
            } 
            movementVector.y += gravityValue * Time.fixedDeltaTime;
            characterController.Move(movementVector * Time.fixedDeltaTime);
        }

        public void ResetGravity()
        {
            if(movementVector.y < 0)
            {
                movementVector.y = -3f;
            }
        }


        #endregion
    }
}

