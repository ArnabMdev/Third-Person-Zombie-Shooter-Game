using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> movePressed;
    public static event Action<double> shootPressed;
    public static event Action<float> pitchChanged;
    public static event Action<float> rollChanged;
    public static event Action<float> yawChanged;
    public static event Action jumpPressed;
    public static event Action crouchPressed;
    public static event Action aimPressed;
    public static event Action toggleSprint;
    public static event Action toggleFlight;

    public void Move(InputAction.CallbackContext context)
    {
        var moveDir = context.ReadValue<Vector2>();
        movePressed?.Invoke(moveDir);
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            jumpPressed?.Invoke();
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            toggleSprint?.Invoke();
        }
        if (context.canceled)
        {
            toggleSprint?.Invoke();
        }
    }

    public void ToggleFlying(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            toggleFlight?.Invoke();
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            crouchPressed?.Invoke();
        }
        if (context.canceled)
        {
            crouchPressed?.Invoke();
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            aimPressed?.Invoke();
        }
        if(context.canceled)
        {
            aimPressed?.Invoke();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            shootPressed?.Invoke(context.duration);
        }
        if(context.canceled)
        {
            shootPressed?.Invoke(0);
        }
    }

    public void ControlPitch(InputAction.CallbackContext context)
    {
        var pitch = context.ReadValue<float>();
        pitchChanged?.Invoke(pitch);
    }

    public void ControlRoll(InputAction.CallbackContext context)
    {
        var roll = context.ReadValue<float>();
        rollChanged?.Invoke(roll);
    }
    public void ControlYaw(InputAction.CallbackContext context)
    {
        var yaw = context.ReadValue<float>();
        yawChanged?.Invoke(yaw);
    }


}
