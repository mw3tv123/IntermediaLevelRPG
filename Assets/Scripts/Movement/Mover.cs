using RPG.Core;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    /// <summary>
    /// Movement control class for move an object.
    /// </summary>
    public class Mover : MonoBehaviour, IAction, ISaveable {
        /// <summary>
        /// An interface where it hold basic movement of all object (Player, NPC).
        /// </summary>
        NavMeshAgent agent;     // Reference to this object's NavMeshAgent for navigating/moving object around.
        Animator animator;      // Reference to this object's Animator for renderring animation clip.

        [SerializeField] float maxSpeed = 6f;

        void Awake() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            // Navigation system only avalable when this object alive.
            agent.enabled = !GetComponent<Health>().IsDeath;
            UpdateAnimator();
        }

        // Use to intercept between action (stop attack then move and via versal).
        public void StartMovement(Vector3 point, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveToPoint(point, speedFraction);
        }

        // Move this object to an point in the world space.
        public void MoveToPoint(Vector3 point, float speedFraction) {
            agent.SetDestination(point);
            agent.isStopped = false;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

        // Constainly update object animator overtime.
        private void UpdateAnimator() {
            float speed = transform.InverseTransformDirection(agent.velocity).z;
            animator.SetFloat("speed", speed);
        }

        // Cancel all movement action.
        public void CancelThisAction() {
            agent.isStopped = true;
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState( object state ) {
            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ToVector3();
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
