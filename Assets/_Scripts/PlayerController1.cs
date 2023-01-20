using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Stateless;
using Stateless.Graph;
using UnityEngine.Serialization;

// ReSharper disable CompareOfFloatsByEqualityOperator


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
        [FormerlySerializedAs("IsGrounded")] public bool isGrounded;
        [HideInInspector] public Animator animator;
        private float _turnSmoothVelocity;
        private Vector3 _movementVector;


        [Header("Gun Properties")]
        [SerializeField] private GameObject gun;
        [SerializeField] private Texture2D xHairImage;
        [SerializeField] private Vector2 xHairOffset;
        public GunMode gunMode = GunMode.Auto;


        [Header("Flight Properties")]
        [SerializeField] private bool canFly;
        [SerializeField] private float propulsionSpeed;
        [SerializeField] private float pitchPower, yawPower, rollPower;
        private float _activePitch, _activeYaw, _activeRoll;
        private Vector3 _moveVector;

        private PlayerStateMachine _playerSm;
        private CharacterController _characterController;
        private static readonly int IsStrafingInDirection = Animator.StringToHash("isStrafingInDirection");

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
            _playerSm = new PlayerStateMachine(this);
            _playerSm.InitializeStates();
            _playerSm.InitializeStateMachine();
        }
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            _moveVector = Vector3.zero;
            _movementVector = Vector3.zero;
            _playerSm.StateMachine.Fire(Trigger.StartedGame);
            //playerSM.stateMachine.State.Entry();

        }

        private void OnGUI()
        {
            if (Application.isEditor)
            {
                var stateLog = GUI.TextArea(new Rect(10, 10, 1000, 100),
                    _playerSm.StateMachine.State.ToString());
            }
        }

        private void FixedUpdate()
        {
            _playerSm.StateMachine.State.UpdateLogic();
            isGrounded = _characterController.isGrounded;

        }
        #endregion

        #region ControllerMethods

        public void PerformJump()
        {
            _movementVector.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            _characterController.Move(_movementVector * Time.fixedDeltaTime);
        }

        public void StrafePlayer(Vector2 moveDir)
        {
            var strafeVector = Vector3.zero;
            if(moveDir.x == 1)
            {
                animator.SetInteger(IsStrafingInDirection, 2);
                strafeVector = transform.right * strafeSpeed;
            }
            else if(moveDir.x == -1)
            {
                animator.SetInteger(IsStrafingInDirection, 3);
                strafeVector = -1 * transform.right * strafeSpeed;
            }
            else if(moveDir.y == 1)
            {
                animator.SetInteger(IsStrafingInDirection, 1);
                strafeVector = transform.forward * strafeSpeed;
            }
            else if(moveDir.y == -1)
            {
                animator.SetInteger(IsStrafingInDirection, 4);
                strafeVector = -1 * transform.forward * strafeSpeed;
            }
            // Debug.Log(strafeVector);
            _characterController.Move(strafeVector);
        }

        public void MovePlayer(Vector2 moveDir, int speedMultiplier)
        {
            TurnPlayer(moveDir);
            float walkTrigger = Mathf.Sqrt(Mathf.Pow(moveDir.x, 2) + Mathf.Pow(moveDir.y, 2));
            if (walkTrigger > turningValueOffset)
            {
                _movementVector = new Vector3(_moveVector.x * moveSpeed * Time.fixedDeltaTime, 0, _moveVector.z * moveSpeed * Time.fixedDeltaTime);
                _characterController.Move(_movementVector * speedMultiplier * Time.fixedDeltaTime); 

            }

        }

        private void TurnPlayer(Vector2 inputVector)
        {
            Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y); ;
            if (inputVector.magnitude >= 0.1f)
            {
                float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                _moveVector = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }
        }



        private void ControlFlight()
        {
            _characterController.Move(transform.up * propulsionSpeed);
            transform.Rotate(_activePitch * pitchPower * Time.fixedDeltaTime, -_activeRoll * Time.fixedDeltaTime, -_activeYaw * yawPower * Time.fixedDeltaTime, Space.Self);
        }

        public void ApplyGravity()
        {
            if(_characterController.isGrounded)
            {
                return;
            } 
            _movementVector.y += gravityValue * Time.fixedDeltaTime;
            _characterController.Move(_movementVector * Time.fixedDeltaTime);
        }

        public void ResetGravity()
        {
            if(_movementVector.y < 0)
            {
                _movementVector.y = -3f;
            }
        }


        #endregion
    }
}

