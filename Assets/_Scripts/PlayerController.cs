using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle = 0,
    Walking = 1,
    Sprinting = 2,
    Crouching = 3,
    Jumping = 4,
    Flying = 5,

}

public class PlayerController : MonoBehaviour
{
#region Properties
    [Header("Movement Properties")]
    public PlayerState activeState;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFlying;
    [SerializeField] private GameObject checkSphereTarget;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkRadius;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] float turningValueOffset;
    private float _turnSmoothVelocity;
    private Vector2 _moveDir;


    [Header("Gun Properties")]
    [SerializeField] private GameObject gun;
    [SerializeField] private Texture2D xHairImage;
    [SerializeField] private Vector2 xHairOffset;


    [Header("Flight Properties")]
    [SerializeField] private bool canFly;
    [SerializeField] private float propulsionSpeed;
    [SerializeField] private float pitchPower, yawPower, rollPower;
    private float _activePitch, _activeYaw, _activeRoll;
    private Vector3 _moveVector;
    private Rigidbody _rbd;

    #endregion

#region UnityMethods
    void Start()
    {
        _rbd = GetComponent<Rigidbody>();
        activeState = PlayerState.Idle;
        _moveVector = Vector3.zero;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(checkSphereTarget.transform.position, checkRadius, groundMask);
        if(activeState == PlayerState.Jumping && isGrounded)
        {
            activeState = PlayerState.Walking;
        }
        switch (activeState)
        {
            case PlayerState.Idle:
                TurnPlayer(_moveDir);
                break;
            case PlayerState.Walking:
                TurnPlayer(_moveDir);
                MovePlayer();
                break;
            case PlayerState.Sprinting:
                TurnPlayer(_moveDir);
                MovePlayer();
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Flying:
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
        _moveDir = context.ReadValue<Vector2>();
        if(activeState == PlayerState.Jumping || activeState == PlayerState.Flying)
        {
            return;
        }
        if (context.performed)
        {
            activeState = PlayerState.Walking;
        }
        if (context.canceled)
        {
            activeState = PlayerState.Idle;
        }


    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (activeState == PlayerState.Jumping || activeState == PlayerState.Flying)
        {
            return;
        }
        activeState = PlayerState.Jumping;
        PerformJump();
        

    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (activeState == PlayerState.Flying || activeState == PlayerState.Jumping)
        {
            return;
        }
        if(context.started)
        {
            activeState = PlayerState.Sprinting;
        }
        if(context.canceled)
        {
            activeState = PlayerState.Walking;
        }
    }

    public void ToggleFlying(InputAction.CallbackContext context)
    {
        if (!canFly)
            return;
        if (context.performed)
        {
            if (activeState == PlayerState.Flying)
            {
                activeState = PlayerState.Walking;
                _rbd.useGravity = true;
                _rbd.velocity = Vector3.zero;
                transform.Rotate(Vector3.right, -90);
            }
            else
            {
                activeState = PlayerState.Flying;
                _rbd.useGravity = false;
                transform.Rotate(Vector3.right, 90);
            }
        }
    }


    public void ControlPitch(InputAction.CallbackContext context)
    {
        if (activeState != PlayerState.Flying)
            return;
        Debug.Log("pitch");
        _activePitch = context.ReadValue<float>();
        _activePitch *= pitchPower;
    }

    public void ControlRoll(InputAction.CallbackContext context)
    {
        if (activeState != PlayerState.Flying)
            return;
        _activeRoll = context.ReadValue<float>();
        _activeRoll *= rollPower;
    }

    public void ControlYaw(InputAction.CallbackContext context)
    {
        if (activeState != PlayerState.Flying)
            return;
        _activeYaw = context.ReadValue<float>();
        _activeYaw *= yawPower;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (activeState == PlayerState.Flying || activeState == PlayerState.Jumping)
        {
            return;
        }
        if (context.started)
        {
            activeState = PlayerState.Crouching;
        }
        if (context.canceled)
        {
            activeState = PlayerState.Crouching;
        }
    }

    #endregion

#region ControllerMethods
    private void PerformJump()
    {
        if (isGrounded)
            _rbd.AddForce(Vector3.up * jumpForce);

    }

    private void MovePlayer()
    {
        float walkTrigger = Mathf.Sqrt(Mathf.Pow(_moveDir.x, 2) + Mathf.Pow(_moveDir.y, 2));
        if (walkTrigger > turningValueOffset)
        {
            _rbd.velocity = new Vector3(_moveVector.x * moveSpeed * Time.fixedDeltaTime, _rbd.velocity.y, _moveVector.z * moveSpeed * Time.fixedDeltaTime);

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
        _rbd.velocity = transform.up * propulsionSpeed;
        transform.Rotate(_activePitch * pitchPower * Time.fixedDeltaTime, -_activeRoll * Time.fixedDeltaTime, _activeYaw * yawPower * Time.fixedDeltaTime, Space.Self);   
    }

#endregion
}
