using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class InputManager : MonoBehaviour
    {
        public static Vector2 moveDir { get; private set; }
        public static float pitch { get; private set; }
        public static float yaw { get; private set; }
        public static float roll { get; private set; }
        public static event Action movePressed;
        public static event Action shootPressed;
        public static event Action pitchChanged;
        public static event Action rollChanged;
        public static event Action yawChanged;
        public static event Action jumpPressed;
        public static event Action crouchPressed;
        public static event Action aimPressed;
        public static event Action toggleSprint;
        public static event Action toggleFlight;

        public void Move(InputAction.CallbackContext context)
        {
            var moveDir = context.ReadValue<Vector2>();
            movePressed?.Invoke();

        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
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
            if (context.performed)
            {
                aimPressed?.Invoke();
            }
            if (context.canceled)
            {
                aimPressed?.Invoke();
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                shootPressed?.Invoke();
            }
            if (context.canceled)
            {
                shootPressed?.Invoke();
            }
        }

        public void ControlPitch(InputAction.CallbackContext context)
        {
            pitch = context.ReadValue<float>();
            if (context.performed)
                pitchChanged?.Invoke();
        }

        public void ControlRoll(InputAction.CallbackContext context)
        {
            roll = context.ReadValue<float>();
            if (context.performed)
                rollChanged?.Invoke();
        }
        public void ControlYaw(InputAction.CallbackContext context)
        {
            yaw = context.ReadValue<float>();
            if (context.performed)
                yawChanged?.Invoke();
        }


    } 
}
