using UnityEngine;
using UnityEngine.AI;

namespace Movement {
    public class Mover : MonoBehaviour {
        [SerializeField]
        private Transform target;

        private NavMeshAgent agent;

        private void Start ( ) {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        private void Update ( ) {
            if ( Input.GetMouseButton(0) ) {
                MoveToCursor();
            }
            UpdateAnimator();
        }

        private void MoveToCursor ( ) {
            // Get a ray from camera to mouse point.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casting the ray to a hit.
            bool isHit = Physics.Raycast(ray, out RaycastHit hit);
            if ( isHit ) {  // If the ray hit something...
                agent.destination = hit.point;
            }
        }

        private void UpdateAnimator ( ) {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("Speed", speed);
        }
    }
}