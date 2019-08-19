using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control {
    [RequireComponent(typeof(Movement.Mover))]
    public class PlayerController : MonoBehaviour {
        [Range(0, 1), SerializeField] float runSpeedFraction = 1f;

        Mover mover;
        Fighter combat;
        Health health;

        // Start is called before the first frame update
        void Start() {
            mover = GetComponent<Mover>();
            combat = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update() {
            // If this Player is DEAD, he can't control anything else...
            if (health.IsDeath) return;
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
                    combat.Attack(target.gameObject);
                return true;
            }
            return false;
        }

        // Casting a ray to the point where Player clicked and move to that point.
        private bool Move() {
            if (Input.GetMouseButton(0)) {
                Ray controller_ray = GetRayToMouse();
                if (Physics.Raycast(controller_ray, out RaycastHit target, 50)) {
                    mover.StartMovement(target.point, runSpeedFraction);
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