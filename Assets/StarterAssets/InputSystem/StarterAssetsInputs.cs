using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {


        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
        public bool mouseLC = false;
        public bool mouseRC = false;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        private void Awake()
        {
            
        }
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
                JumpInput(true);
            if (value.canceled)
                JumpInput(false);
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            if (value.performed)
                SprintInput(true);
            if (value.canceled)
                SprintInput(false);

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

        }


        public void OnMouseRC(InputAction.CallbackContext newMouseRC)
        {
            if (newMouseRC.performed)
            {
                mouseRC = true;
            }
            if (newMouseRC.canceled)
            {
                mouseRC = false;
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