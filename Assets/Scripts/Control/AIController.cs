using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control {
    // This use to control Mods which automatic aim at Player.
    public class AIController : MonoBehaviour {
        [SerializeField] readonly float viewRadius = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] Vector3 guardPosition;
        [SerializeField] PatrolRoute patrolRoute;
        [SerializeField] int lastWaypointIndex = 0;

        Fighter fighter;
        GameObject player;

        float timeLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        void Start() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            guardPosition = transform.position;
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

        // Update internal timer.
        private void UpdateTime() {
            timeLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackMode() {
            // Active ATTACK action and reset timer. Why reset? Because Players are in our sight.
            fighter.Attack(player);
            timeLastSawPlayer = 0f;
        }

        private void SuspisionMode() {
            // Now Players are get out of our sight. We only move to Players's last known position,
            // stay there for few sec to... suspision(?), then get back to Patrol Mode.
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        // In patrol mode, this object is either stand guard at a specific position or moving in a patrol route if has one.
        private void PatrolMode() {
            // Cancel all attack or moving action.
            fighter.CancelThisAction();
            Vector3 nextPosition = guardPosition;

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
                GetComponent<Mover>().StartMovement(nextPosition);
        }

        // TODO: Check logic again in this code.
        // Get the nearest waypoint if this object have a patrol route attach to it.
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

        // Check if Players are in this NPC's view radius.
        private bool InAttackRange() {
            float distance_to_player = Vector3.Distance(player.transform.position, transform.position);
            return distance_to_player <= viewRadius;
        }

        // Call automatic by Unity.
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;                              // Set gizmos color.
            Gizmos.DrawWireSphere(transform.position, viewRadius);  // Draw a sphere which has its center point is this object.
        }
    }
}