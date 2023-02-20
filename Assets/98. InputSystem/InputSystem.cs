using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemAssets
{
    public class InputSystem : MonoBehaviour
    {
        [Header("Character Input Values")] public Vector2 move;
        public bool jump;
        public bool sprint;
        
        [Header("Movement Settings")]
        public bool analogMovement;

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SpintInput(value.isPressed);
        }


        void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        void SpintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }
    }
}