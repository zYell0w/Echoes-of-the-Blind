using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class CharacterInput : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool attack;
        public bool interact;
        public bool cursorLocked = true;
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        public void OnAttack(InputValue value)
        {
            AttackInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }




        public void MoveInput(Vector2 newMoveDir)
        {
            move = newMoveDir;
        }

        public void LookInput(Vector2 newLookDir)
        {
            look = newLookDir;
        }

        public void AttackInput(bool newAttack)
        {
            attack = newAttack;
        }

        public void InteractInput(bool newInteract)
        {
            interact = newInteract;
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
