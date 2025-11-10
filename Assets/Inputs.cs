using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class CharacterInput : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public float attack;
        public float interact;
        public float scan;

        public bool cursorLocked = true;

        public float walk;
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
            AttackInput(value.Get<float>());
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.Get<float>());
        }

        public void OnWalk(InputValue value)
        {
            WalkInput(value.Get<float>());
        }

        public void OnScan(InputValue value)
        {
            ScanInput(value.Get<float>());
        }


        public void MoveInput(Vector2 newMoveDir)
        {
            move = newMoveDir;
        }

        public void LookInput(Vector2 newLookDir)
        {
            look = newLookDir;
        }

        public void AttackInput(float newAttack)
        {
            attack = newAttack;
        }

        public void InteractInput(float newInteract)
        {
            interact = newInteract;
        }
        
        public void WalkInput(float newWalk)
        {
            walk = newWalk;
        }

        public void ScanInput(float newScan)
        {
            scan = newScan;
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
