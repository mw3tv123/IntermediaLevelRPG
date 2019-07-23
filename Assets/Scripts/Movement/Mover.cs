using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {
        /// <summary>
        /// An interface where it hold basic movement of all object (Player, NPC).
        /// </summary>
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

        public void StartMovement(Vector3 point) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveToPoint(point);
        }

        public void MoveToPoint(Vector3 point) {
            agent.SetDestination(point);
            agent.isStopped = false;
        }

        // Constainly update object animator overtime.
        private void UpdateAnimator() {
            float speed = transform.InverseTransformDirection(agent.velocity).z;
            animator.SetFloat("speed", speed);
        }

        public void Cancel() {
            agent.isStopped = true;
        }
    }
}
