using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    [RequireComponent(typeof(Movement.Mover))]
    public class PlayerController : MonoBehaviour {
        Mover mover;
        Fighter combat;

        // Start is called before the first frame update
        void Start() {
            mover = GetComponent<Mover>();
            combat = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update() {
            if (Interact()) return;
            if (Move()) return;
        }

        // Interact with an object.
        private bool Interact() {
            RaycastHit[] hits = Physics.RaycastAll(GetRayToMouse());
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null)
                    continue;
                if (Input.GetMouseButtonDown(0))
                    combat.Attack(target);
                return true;
            }
            return false;
        }

        // Casting a ray to the point where Player clicked and move to that point.
        private bool Move() {
            if (Input.GetMouseButton(0)) {
                Ray controller_ray = GetRayToMouse();
                if (Physics.Raycast(controller_ray, out RaycastHit target, 50)) {
                    mover.StartMovement(target.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetRayToMouse() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}