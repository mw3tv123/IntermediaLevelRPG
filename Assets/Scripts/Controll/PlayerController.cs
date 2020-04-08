using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    /// <summary>
    /// Manage player basic control.
    /// </summary>
    public class PlayerController : MonoBehaviour {
        Ray lastRay;

        private void Update ( ) {
            DebugRayCast(lastRay);
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
                    if ( Input.GetMouseButtonDown(0) ) {
                        lastRay = GetRay();
                        GetComponent<Fighter>().Target = target.GetComponent<Health>();
                    }
                    if ( GetComponent<Fighter>().CanNotAttack ) continue;
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
                    GetComponent<Mover>().MoveTo(hit.point);
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