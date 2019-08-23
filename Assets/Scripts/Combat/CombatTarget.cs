using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    public class CombatTarget : MonoBehaviour, IRaycastable {
        public CursorType GetCursorType() {
            return CursorType.Combat;
        }

        public bool HandleRayCast( PlayerController player ) {
            // This will handle attack from the player.
            if ( Input.GetMouseButtonDown(0) )
                player.GetComponent<Fighter>().Attack(gameObject);
            return true;
        }
    }
}

