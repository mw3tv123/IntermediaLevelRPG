using UnityEngine;

namespace RPG.Control {
    // Visualising waypoints of a Patrol Route.
    public class PatrolRoute : MonoBehaviour {
        float waypointZone = 1f;
        void OnDrawGizmos() {
            // Set color for easy visualising.
            Gizmos.color = Color.gray;

            // Iterating throught all waypoint...
            for (int i = 0; i < transform.childCount; i++) {
                // Draw a gray sphere at each waypoint.
                Gizmos.DrawSphere(GetCurrentWaypointPosition(i), 0.5f);
                // Draw a line between current waypoint and the next waypoint.
                Gizmos.DrawLine(GetCurrentWaypointPosition(i), GetCurrentWaypointPosition(GetNextWaypoint(i)));
            }
        }

        public Vector3 GetCurrentWaypointPosition(int i) {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypoint(int i) {
            if (i == transform.childCount - 1)
                return 0;
            return i + 1;
        }

        public bool InPatrolRoute(Transform objectTransform, int waypointIndex) {
            return Vector3.Distance(objectTransform.position, GetCurrentWaypointPosition(waypointIndex)) < waypointZone;
        }
    }
}
