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
        public static bool isAiming { get; private set; }
        public static bool isShooting { get; private set; }
        public static bool isRunning { get; private set; }
        public static event Action reloadPressed;
        public static event Action pitchChanged;
        public static event Action rollChanged;
        public static event Action yawChanged;
        public static event Action jumpPressed;
        public static event Action crouchPressed;
        public static event Action flyPressed;

        public void Move(InputAction.CallbackContext context)
        {
            moveDir = context.ReadValue<Vector2>();

        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpPressed?.Invoke();
            }
        }

        public void Reload(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                reloadPressed?.Invoke();
            }
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isRunning = true;
            }
            if (context.canceled)
            {
                isRunning = false;

            }

        }

        public void ToggleFlying(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                flyPressed?.Invoke();
            }
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                crouchPressed?.Invoke();
            }
        }

        public void Aim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isAiming = true;
            }
            if (context.canceled)
            {
                isAiming = false;
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isShooting = true;
            }
            if (context.canceled)
            {
                isShooting = false;
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
