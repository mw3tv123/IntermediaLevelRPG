using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    /// <summary>
    /// Manage player basic control.
    /// </summary>
    public class PlayerController : MonoBehaviour {
        private Ray lastRay;
        private Fighter fighter;
        private Health self;

        private void Start ( ) {
            fighter = GetComponent<Fighter>();
            self = GetComponent<Health>();
        }

        private void Update ( ) {
            DebugRayCast(lastRay);          // Debug purpose.
            if ( self.IsDead ) return;      // If player dead, player can't do anything.
            if ( Combatable() ) return;
            if ( Moveable() ) return;
        }

        /// <summary>
        /// Detect if target is attackable.
        /// </summary>
        /// <returns>True is target is attackable, otherwise return false.</returns>
        private bool Combatable ( ) {
            RaycastHit[] rays = Physics.RaycastAll(GetRay());
            foreach ( RaycastHit hit in rays ) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if ( target != null ) {
                    if ( Input.GetMouseButton(0) ) {
                        lastRay = GetRay();
                        fighter.StartFightAction(target.GetComponent<Health>());
                    }
                    if ( fighter.CanNotAttack ) continue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Detect if target point is movable.
        /// </summary>
        /// <returns>True if target point is movable, otherwise return false.</returns>
        private bool Moveable ( ) {
            Ray ray = GetRay();
            bool IsHit = Physics.Raycast(ray, out RaycastHit hit);
            if ( IsHit ) {
                if ( Input.GetMouseButton(0) ) {
                    lastRay = ray;
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a ray from camera to mouse point.
        /// </summary>
        private static Ray GetRay ( ) => Camera.main.ScreenPointToRay(Input.mousePosition);

        // For debug purpose only.
        private void DebugRayCast ( Ray ray ) => Debug.DrawRay(ray.origin, ray.direction * 25); 
    }
}