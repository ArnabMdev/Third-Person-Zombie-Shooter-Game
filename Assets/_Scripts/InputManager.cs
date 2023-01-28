using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.Arnab.ZombieAppocalypseShooter
{
    public class InputManager : MonoBehaviour
    {
        public static Vector2 MoveDir { get; private set; }
        public static Vector2 LookDir { get; private set; }
        public static float Pitch { get; private set; }
        public static float Yaw { get; private set; }
        public static float Roll { get; private set; }
        public static bool IsAiming { get; private set; }
        public static bool IsShooting { get; private set; }
        public static bool IsRunning { get; private set; }
        public static event Action ReloadPressed;
        public static event Action PitchChanged;
        public static event Action RollChanged;
        public static event Action YawChanged;
        public static event Action JumpPressed;
        public static event Action CrouchPressed;
        public static event Action FlyPressed;

        public void Move(InputAction.CallbackContext context)
        {
            MoveDir = context.ReadValue<Vector2>();

        }

        public void Look(InputAction.CallbackContext context)
        {
            LookDir = context.ReadValue<Vector2>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpPressed?.Invoke();
            }
        }

        public void Reload(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ReloadPressed?.Invoke();
            }
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsRunning = true;
            }
            if (context.canceled)
            {
                IsRunning = false;

            }

        }

        public void ToggleFlying(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                FlyPressed?.Invoke();
            }
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CrouchPressed?.Invoke();
            }
        }

        public void Aim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAiming = true;
            }
            if (context.canceled)
            {
                IsAiming = false;
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsShooting = true;
            }
            if (context.canceled)
            {
                IsShooting = false;
            }
        }

        public void ControlPitch(InputAction.CallbackContext context)
        {
            Pitch = context.ReadValue<float>();
            if (context.performed)
                PitchChanged?.Invoke();
        }

        public void ControlRoll(InputAction.CallbackContext context)
        {
            Roll = context.ReadValue<float>();
            if (context.performed)
                RollChanged?.Invoke();
        }
        public void ControlYaw(InputAction.CallbackContext context)
        {
            Yaw = context.ReadValue<float>();
            if (context.performed)
                YawChanged?.Invoke();
        }


    } 
}
