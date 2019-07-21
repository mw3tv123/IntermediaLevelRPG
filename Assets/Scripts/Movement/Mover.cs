using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour {
        NavMeshAgent agent;     // Reference to this object's NavMeshAgent for navigating/moving object around.
        Animator animator;      // Reference to this object's Animator for renderring animation clip.

        void Start() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            UpdateAnimator();
        }

        public void MoveToPoint(Vector3 point) { agent.SetDestination(point); }

        private void UpdateAnimator() {
            float speed = transform.InverseTransformDirection(agent.velocity).z;
            animator.SetFloat("speed", speed);
        }
    }
}
