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
    public enum PlayerState
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
        [SerializeField] private float speedChangeRate = 10.0f;
        [SerializeField] private float groundedOffset;
        [SerializeField] private float groundedRadius = 0.28f;
        [SerializeField] private LayerMask groundLayers;
        [Range(0.0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float strafeSpeed;
        [SerializeField]private float jumpTimeout = 0.50f;
        [SerializeField]private float fallTimeout = 0.15f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private bool isFlying;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private GameObject cinemachineCameraTarget;
        [SerializeField] private float topClamp = 70.0f;
        [SerializeField] private float bottomClamp = -30.0f;
        [SerializeField] private float cameraAngleOverride = 0.0f;
        
        public bool isGrounded;
        [HideInInspector] public Animator animator;
        private float _turnSmoothVelocity;
        private Vector3 _movementVector;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private readonly float _terminalVelocity = 53.0f;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private readonly double _threshold = 0.01f;
        [SerializeField] private float yawSensitivity;
        [SerializeField] private float pitchSensitivity;
        [SerializeField] private bool invertYaw;
        [SerializeField] private bool invertPitch;


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
        private float _animationBlend;
        private PlayerStateMachine _playerSm;
        private CharacterController _characterController;
        private static readonly int IsStrafingInDirection = Animator.StringToHash("isStrafingInDirection");

        #endregion

        #region UnityMethods
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
        private void Update()
        {
            GroundedCheck();
            _playerSm.StateMachine.State.UpdateLogic();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        #endregion

        #region ControllerMethods
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }
        public void JumpAndGravity(bool jumps)
        {
            if (isGrounded)
            {
                _fallTimeoutDelta = fallTimeout;
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
                if (jumps && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
                }
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = jumpTimeout;
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += gravityValue * Time.deltaTime;
            }
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
            float targetSpeed = moveSpeed * speedMultiplier * 2;
            if (moveDir == Vector2.zero) 
                targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * (moveDir.magnitude),Time.deltaTime * speedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f)
                _animationBlend = 0f;
            Vector3 inputDirection = new Vector3(moveDir.x, 0.0f, moveDir.y).normalized;


            if (moveDir != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (InputManager.LookDir.sqrMagnitude >= _threshold )
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;

                _cinemachineTargetYaw += InputManager.LookDir.x * yawSensitivity;
                _cinemachineTargetPitch += InputManager.LookDir.y * pitchSensitivity;

                _cinemachineTargetYaw *= invertYaw ? -1 : 1;
                _cinemachineTargetPitch *= invertPitch ? -1 : 1;
                

            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,_cinemachineTargetYaw, 0.0f);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
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
        #endregion
    }
}

