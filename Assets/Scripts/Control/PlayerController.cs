using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    [RequireComponent(typeof(Movement.Mover))]
    public class PlayerController : MonoBehaviour {
        Mover mover;
        Ray controller_ray;

        // Start is called before the first frame update
        void Start() {
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButton(0))
                Move();
        }

        // Casting a ray to the point where Player clicked and move to that point.
        void Move() {
            controller_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(controller_ray, out RaycastHit target, 50)) {
                mover.MoveToPoint(target.point);
            }
        }
    }
}