using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : PlayerState
    {


        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool lookOn;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
        public bool mouseLC = false;
        public bool mouseRC = false;



#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        public void OnMove(InputAction.CallbackContext value)
        {

            MoveInput(value.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed)
            { 
                JumpInput(true);
                GetPlayerBattle(playerState.Jump, move);
            }
            if (value.canceled)
            {
                JumpInput(false);
            
            }
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            if (value.performed)
            { 
                SprintInput(true);
                GetPlayerBattle(playerState.Dash, move);
            }
            if (value.canceled)
            { 
                SprintInput(false);
            
            }

        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;

        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void OnMouseLC(InputAction.CallbackContext newMouseLC)
        {
            if (newMouseLC.performed)
            {
                GetPlayerBattle(playerState.Attack, move);
            }
            if (newMouseLC.canceled)
            {
               
            }
        }


        public void OnMouseRC(InputAction.CallbackContext newMouseRC)
        {
            if (newMouseRC.performed)
            {
                GetPlayerBattle(playerState.Guard, move);
            }
            
            if (newMouseRC.canceled)
            {
                GetPlayerBattle(playerState.NonGuard, move);

            }

        }
        public void OnLockOn(InputAction.CallbackContext newLockOn)
        {
            if (newLockOn.started)
            {
                lookOn = true;
      
            }
            if (newLockOn.canceled)
            {
                lookOn = false;
            }


        }


        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}