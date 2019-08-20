using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using GameDevTV.Utils;

namespace RPG.Control {
    /// <summary>
    /// This use to control Mods which automatic aim at Player.
    /// </summary>
    public class AIController : MonoBehaviour {
        [SerializeField] float viewRadius = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolRoute patrolRoute;
        [SerializeField] int lastWaypointIndex = 0;

        [Range(0, 1), SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        GameObject player;
        LazyValue<Vector3> guardPosition;

        float timeLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        void Awake() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");

            // Initiate default guard position for guard.
            guardPosition = new LazyValue<Vector3>( () => transform.position );
        }

        void Start() {
            guardPosition.ForceInit();
        }

        void Update() {
            // If this NPC is DEAD, he can't do anything else...
            if (GetComponent<Health>().IsDeath) return;

            // Attack behavior.
            // If Players are in this NPC view radius, attack them...
            if (InAttackRange())
                AttackMode();

            // Suspicion behavior.
            // Lost sigh of Player, become suspicion mode.
            else if (timeLastSawPlayer < suspicionTime)
                SuspisionMode();

            // Guard behavior.
            // ...else stand guarding.
            else
                PatrolMode();

            UpdateTime();
        }

        /// <summary>
        /// Update internal timer.
        /// </summary>
        private void UpdateTime() {
            timeLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackMode() {
            // Active ATTACK action and reset timer. Why reset? Because Players are in our sight.
            fighter.Attack(player);
            timeLastSawPlayer = 0f;
        }

        /// <summary>
        /// Now Players are get out of our sight. We only move to Players's last known position,
        /// stay there for few sec to... suspision(?), then get back to Patrol Mode.
        /// </summary>
        private void SuspisionMode() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        /// <summary>
        /// In patrol mode, this object is either stand guard at a specific position or moving in a patrol route if has one.
        /// </summary>
        private void PatrolMode() {
            // Cancel all attack or moving action.
            fighter.CancelThisAction();
            Vector3 nextPosition = guardPosition.value;

            // If this object has a patrol route...
            if (patrolRoute != null) {
                // ...check if this object has visit this waypoint recently...
                if (patrolRoute.InPatrolRoute(transform, lastWaypointIndex)) {
                    lastWaypointIndex = patrolRoute.GetNextWaypoint(lastWaypointIndex);
                    timeSinceArrivedAtWaypoint = 0;
                }

                // ...moving to next waypoint.
                nextPosition = patrolRoute.GetCurrentWaypointPosition(lastWaypointIndex);
            }

            // Stay at each waypoint at a period time.
            if (timeSinceArrivedAtWaypoint > suspicionTime)
                GetComponent<Mover>().StartMovement(nextPosition, patrolSpeedFraction);
        }

        // TODO: Check logic again in this code.
        /// <summary>
        /// Get the nearest waypoint if this object have a patrol route attach to it.
        /// </summary>
        /// <param name="routes"></param>
        private void GetNearestWaypoint(Transform routes) {
            // Initialize a nearest waypoint and shortest distance.
            Vector3 nearest_point = routes.GetChild(0).position;
            float shortest_distance = Vector3.Distance(transform.position, nearest_point);

            // Check if another waypoint is closer than current nearest_point.
            for (int i = 1; i < routes.childCount; i++) {
                if (Vector3.Distance(transform.position, routes.GetChild(i).position) < shortest_distance) {
                    lastWaypointIndex = i;
                    shortest_distance = Vector3.Distance(transform.position, routes.GetChild(i).position);
                    nearest_point = routes.GetChild(i).position;
                }
            }
        }

        /// <summary>
        /// Check if Players are in this NPC's view radius.
        /// </summary>
        private bool InAttackRange() {
            float distance_to_player = Vector3.Distance(player.transform.position, transform.position);
            return distance_to_player <= viewRadius;
        }

        /// <summary>
        /// Draw a wire sphere to demonstrate this object's field of view.
        /// </summary>
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;                              // Set gizmos color.
            Gizmos.DrawWireSphere(transform.position, viewRadius);  // Draw a sphere which has its center point is this object.
        }
    }
}