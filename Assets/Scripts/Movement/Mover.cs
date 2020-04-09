using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    /// <summary>
    /// Manage basic an object basic movement like idle, run, walk, jump, climb,...
    /// </summary>
    public class Mover : MonoBehaviour, IAction {
        private NavMeshAgent agent;

        private void Start ( ) {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        private void Update ( ) {
            UpdateAnimator();
        }

        /// <summary>
        /// Start and register move action to Action Scheduler.
        /// </summary>
        /// <param name="destination">Point where this object will move to.</param>
        public void StartMoveAction ( Vector3 destination ) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        /// <summary>
        /// Move to destination point.
        /// </summary>
        /// <param name="destination">Point where this object will move to.</param>
        public void MoveTo ( Vector3 destination ) {
            agent.isStopped = false;
            agent.destination = destination;
        }

        public void Cancel ( ) => agent.isStopped = true;

        /// <summary>
        /// Animate this object's animation.
        /// </summary>
        private void UpdateAnimator ( ) {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("Speed", speed);
        }
    }
}